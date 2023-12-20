
namespace ChatApp.Domain.Chat.Request
{
    public class SendMessageRequest
    {
        public Guid RoomId { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
    }
}
