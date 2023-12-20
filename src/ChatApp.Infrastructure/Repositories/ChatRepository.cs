using ChatApp.Domain.Entities.Command;
using ChatApp.Infrastructure.Database.Command;
using ChatApp.Infrastructure.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Repositories
{
    public class ChatRepository : EfRepository<ChatRoom>, IChatRepository
    {
        public ChatRepository(ChatDbContext context) : base(context)
        {
        }

        public async Task<ChatRoom> GetByIdWithMessagesAsync(Guid id)
        {
            var room = await GetAll().Include(x => x.Messages).FirstOrDefaultAsync(x => x.Id == id);
            return room;
        }
    }
}
