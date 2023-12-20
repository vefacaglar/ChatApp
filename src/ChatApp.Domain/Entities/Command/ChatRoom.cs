using System.ComponentModel.DataAnnotations;

namespace ChatApp.Domain.Entities.Command
{
    public class ChatRoom : BaseEntity<Guid>, IAggregateRoot
    {
        [MaxLength(50)]
        public string Name { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public ICollection<RoomMessage> Messages { get; set; }

        public ChatRoom(string name)
        {
            name = name ?? throw new ArgumentNullException(nameof(name));
            CreatedAt = DateTime.Now;
        }

        public void AddMessage(string userName, string message)
        {
            var roomMessage = new RoomMessage(userName, message);

            Messages.Add(roomMessage);
        }
    }
}
