using ChatApp.Domain.Database.ChatDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Domain.Database.ChatDb
{
    public partial class ChatDbContext : DbContext
    {
        public ChatDbContext()
        {

        }

        public ChatDbContext(DbContextOptions<ChatDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChatRoom> ChatRoom { get; set; }
        public virtual DbSet<EventStore> EventStore { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
