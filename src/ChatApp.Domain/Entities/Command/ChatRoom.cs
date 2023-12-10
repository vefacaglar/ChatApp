using System.ComponentModel.DataAnnotations;

namespace ChatApp.Domain.Entities.Command
{
    public class ChatRoom : IEntity
    {
        public Guid Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
