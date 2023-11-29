using ChatApp.Domain.Database.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Domain.Database.ChatDb.Entities
{
    public class EventLog : IEntity
    {
        public EventLog()
        {
            CreatedAt = DateTime.Now;
        }

        public long Id { get; set; }

        [MaxLength(200)]
        public string Type { get; set; }

        [Column(TypeName = "text")]
        public string Payload { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
