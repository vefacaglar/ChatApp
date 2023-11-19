namespace ChatApp.Infrastructure
{
    public class ChatAppConfiguration
    {
        public int RetryCount { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
    }

    public class ConnectionStrings
    {
        public string ChatDbCommand { get; set; }
        public string ChatDbRead { get; set; }
        public EventBusSetting EventBus { get; set; }
    }

    public class EventBusSetting
    {
        public string Connection { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
