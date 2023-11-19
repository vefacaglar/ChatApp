using ChatApp.Domain.Database.ChatDb.Entities;
using ChatApp.Application.Command;
using ChatApp.Infrastructure;

namespace ChatApp.Application.Chat
{
    public class CreateChatRoomCommand : ICommand<CreateChatRoomCommandResult>
    {
        public string Name { get; set; }

        public CreateChatRoomCommand()
        {
        }
    }

    public class CreateChatRoomCommandResult : CommandResult
    {
        public string Code { get; set; }
    }

    public class CreateChatRoomCommandHandler : ICommandHandler<CreateChatRoomCommand, CreateChatRoomCommandResult>
    {
        private readonly IUnitOfWork _uow;

        public CreateChatRoomCommandHandler(
            IUnitOfWork uow
            )
        {
            _uow = uow;
        }

        public async Task<CreateChatRoomCommandResult> Handle(CreateChatRoomCommand command)
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
