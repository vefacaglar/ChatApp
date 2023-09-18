﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace ChatApp.Application.EventBus
{
    public class RabbitMqPersistentConnection : IRabbitMqPersistentConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        EventBusRabbitMq _eventBusRabbitMq;
        IConnection _connection;
        bool _disposed;

        public RabbitMqPersistentConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            if (!IsConnected)
            {
                TryConnect();
            }
        }

        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }

        public void CreateConsumerChannel()
        {
            if (!IsConnected)
            {
                TryConnect();
            }

            _eventBusRabbitMq = new EventBusRabbitMq(this, "chatroom.sendMessage");
            _eventBusRabbitMq.CreateConsumerChannel();
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }
            return _connection.CreateModel();
        }

        public void Disconnect()
        {
            if (_disposed)
            {
                return;
            }
            Dispose();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            try
            {
                _connection.Dispose();
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public bool TryConnect()
        {

            try
            {
                Console.WriteLine("RabbitMQ Client is trying to connect");
                _connection = _connectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException e)
            {
                Thread.Sleep(5000);
                Console.WriteLine("RabbitMQ Client is trying to reconnect");
                _connection = _connectionFactory.CreateConnection();
            }

            if (IsConnected)
            {
                _connection.ConnectionShutdown += OnConnectionShutdown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;

                Console.WriteLine($"RabbitMQ persistent connection acquired a connection {_connection.Endpoint.HostName} and is subscribed to failure events");

                return true;
            }
            else
            {
                //  implement send warning email here
                //-----------------------
                Console.WriteLine("FATAL ERROR: RabbitMQ connections could not be created and opened");
                return false;
            }

        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;
            Console.WriteLine("A RabbitMQ connection is shutdown. Trying to re-connect...");
            TryConnect();
        }

        void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;
            Console.WriteLine("A RabbitMQ connection throw exception. Trying to re-connect...");
            TryConnect();
        }

        void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (_disposed) return;
            Console.WriteLine("A RabbitMQ connection is on shutdown. Trying to re-connect...");
            TryConnect();
        }
    }
}
