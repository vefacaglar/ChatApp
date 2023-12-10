using ChatApp.Infrastructure;
using ChatApp.Infrastructure.Transactions;

namespace ChatApp.Application.EventHandler
{
    public class CreatedChatRoomEventHandler : IEventHandler<EventCreatedChatRoom>
    {
        public CreatedChatRoomEventHandler()
        {
            
        }

        public async Task Handle(EventCreatedChatRoom e)
        {
            // syn read database
        }
    }
}
