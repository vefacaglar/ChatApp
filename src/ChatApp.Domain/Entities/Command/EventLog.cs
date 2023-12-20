using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Domain.Entities.Command
{
    public class EventLog : BaseEntity<long>
    {
        public EventLog()
        {
            CreatedAt = DateTime.Now;
        }


        [MaxLength(200)]
        public string Type { get; set; }

        [Column(TypeName = "text")]
        public string Payload { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
