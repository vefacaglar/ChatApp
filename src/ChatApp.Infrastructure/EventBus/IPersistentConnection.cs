namespace ChatApp.Infrastructure.EventBus
{
    public interface IPersistentConnection<T> : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        T CreateModel();
    }
}
