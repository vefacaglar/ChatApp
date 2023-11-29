using ChatApp.Domain.Database.ChatDb.Entities;
using ChatApp.Application.Command;
using ChatApp.Infrastructure;
using ChatApp.Infrastructure.Transactions;
using Newtonsoft.Json;

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
        private readonly IEventBus _eventBus;

        public CreateChatRoomCommandHandler(
            IUnitOfWork uow,
            IEventBus eventBus
            )
        {
            _uow = uow;
            _eventBus = eventBus;
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

            _eventBus.Publish(new EventCreatedChatRoom(new EventLog
            {
                Payload = JsonConvert.SerializeObject(newRoom),
                Type = "CreatedChatRoom",
            }));

            return new CreateChatRoomCommandResult
            {
                Code = newRoom.Id.ToString(),
            };
        }
    }
}
