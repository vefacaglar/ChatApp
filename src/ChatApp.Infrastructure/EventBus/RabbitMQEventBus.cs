using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace ChatApp.Infrastructure.EventBus
{
    public class RabbitMQEventBus : IEventBus, IDisposable
    {
        private readonly string QUEUE_NAME = "chat_events";
        private readonly string BROKER_NAME = "chat_events";

        private readonly IEventDispatcher _eventDispatcher;
        private readonly IPersistentConnection<IModel> _connection;
        private readonly ILogger<RabbitMQEventBus> _logger;
        private readonly static Dictionary<string, Type> _subsManager = new();
        private readonly int _retryCount;

        private IModel _consumerChannel;

        private bool disposedValue = false;

        public RabbitMQEventBus(
            IEventDispatcher eventDispatcher,
            IPersistentConnection<IModel> connection,
            ILogger<RabbitMQEventBus> logger,
            int retryCount = 5
            )
        {
            _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _retryCount = retryCount;
            _consumerChannel = CreateConsumerChannel();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new NonPublicPropertiesResolver()
            };
        }

        public void Publish(IEvent @event)
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)), (ex, time) =>
                {
                    _logger.LogWarning("retrying message because of error {0}", ex.ToString());
                });

            using var channel = _connection.CreateModel();
            var eventName = @event.GetType().Name;

            channel.ExchangeDeclare(exchange: BROKER_NAME, type: "direct");

            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            policy.Execute(() =>
            {
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent

                channel.BasicPublish(exchange: BROKER_NAME,
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body);
            });
        }

        public void Subscribe<T>() where T : IEvent
        {
            var eventName = typeof(T).Name;
            var containsKey = _subsManager.ContainsKey(eventName);

            if (!containsKey)
            {
                _subsManager.Add(eventName, typeof(T));
            }

            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            using var channel = _connection.CreateModel();
            channel.QueueBind(queue: QUEUE_NAME,
                exchange: BROKER_NAME,
                routingKey: eventName);
        }

        private IModel CreateConsumerChannel()
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            var channel = _connection.CreateModel();
            channel.ExchangeDeclare(exchange: BROKER_NAME,
                type: "direct");

            channel.QueueDeclare(queue: QUEUE_NAME,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var eventName = ea.RoutingKey;
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                await ProcessEvent(eventName, message);

                channel.BasicAck(ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: QUEUE_NAME,
                autoAck: false,
                consumer: consumer);

            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
            };

            return channel;
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_subsManager.ContainsKey(eventName))
            {
                var @type = _subsManager[eventName];
                var @event = JsonConvert.DeserializeObject(message, @type) as IEvent;

                await _eventDispatcher.Dispatch(@event);
            }
        }

        public void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(_consumerChannel != null)
                    {
                        _consumerChannel.Dispose();
                    }

                    _subsManager.Clear();
                }
                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
        }

        public class NonPublicPropertiesResolver : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var prop = base.CreateProperty(member, memberSerialization);
                if (member is PropertyInfo pi)
                {
                    prop.Readable = (pi.GetMethod != null);
                    prop.Writable = (pi.SetMethod != null);
                }
                return prop;
            }
        }
    }
}
