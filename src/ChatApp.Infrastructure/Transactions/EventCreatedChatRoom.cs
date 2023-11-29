using ChatApp.Domain.Database.ChatDb.Entities;

namespace ChatApp.Infrastructure.Transactions
{
    public class EventCreatedChatRoom : Event
    {
        public EventLog Data { get; set; }

        public EventCreatedChatRoom(EventLog eventLog)
        {
            Data = eventLog;
        }
    }
}
