using ChatApp.Domain.Database.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Domain.Database.ChatDb.Entities
{
    public class EventStore : IEntity
    {
        public long Id { get; set; }

        [MaxLength(100)]
        public string Code { get; set; }

        [MaxLength(50)]
        public string Type { get; set; }

        [Column(TypeName = "text")]
        public string Payload { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
