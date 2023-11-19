using Azure.Core;
using ChatApp.Domain.Database.ChatDb.Entities;
using ChatApp.Infrastructure;
using ChatApp.Infrastructure.Command;

namespace ChatApp.Application.Chat
{
    public class CreateChatRoomCommand2 : ICommand<CreateChatRoomCommandResult>
    {
        public string Name { get; set; }

        public CreateChatRoomCommand2()
        {
        }
    }

    public class CreateChatRoomCommandResult : CommandResult
    {
        public string Code { get; set; }
    }

    public class CreateChatRoomCommandHandler : ICommandHandler<CreateChatRoomCommand2, CreateChatRoomCommandResult>
    {
        private readonly IUnitOfWork _uow;

        public CreateChatRoomCommandHandler(
            IUnitOfWork uow
            )
        {
            _uow = uow;
        }

        public async Task<CreateChatRoomCommandResult> Handle(CreateChatRoomCommand2 command)
        {
            var newRoom = new ChatRoom()
            {
                Name = command.Name,
                CreatedAt = DateTime.UtcNow,
            };

            var repository = _uow.GetRepository<ChatRoom>();
            repository.Add(newRoom);

            await _uow.SaveChangesAsync();

            return new CreateChatRoomCommandResult
            {
                Code = newRoom.Id.ToString(),
            };
        }
    }
}
