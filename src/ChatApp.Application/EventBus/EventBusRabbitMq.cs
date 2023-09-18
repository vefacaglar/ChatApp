using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ChatApp.Application.EventBus
{
    public class EventBusRabbitMq
    {
        private readonly IRabbitMqPersistentConnection _connection;
        private IModel _consumerChannel;
        private string _queueName;

        public EventBusRabbitMq(
            IRabbitMqPersistentConnection persistentConnection,
            string queueName
            )
        {
            _connection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _queueName = queueName;
        }

        public IModel CreateConsumerChannel()
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            var channel = _connection.CreateModel();
            channel.QueueDeclare(
                queue: _queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += ReceivedEvent;

            channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
            };

            return channel;
        }

        private void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            if (e.RoutingKey == "chatroom.sendMessage")
            {
                // implementing here
            }
        }

        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }
        }
    }
}
