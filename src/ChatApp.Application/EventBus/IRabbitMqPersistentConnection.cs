using RabbitMQ.Client;

namespace ChatApp.Application.EventBus
{
    public interface IRabbitMqPersistentConnection
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();

        void CreateConsumerChannel();

        void Disconnect();
    }
}
