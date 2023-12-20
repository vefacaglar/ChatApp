using ChatApp.Domain.Entities;
using ChatApp.Application.Command;
using ChatApp.Infrastructure;
using ChatApp.Infrastructure.Transactions;
using Newtonsoft.Json;
using ChatApp.Domain.Entities.Command;

namespace ChatApp.Application.Chat
{
    public class CreateChatRoomCommand : ICommand<CreateChatRoomCommandResult>
    {
        public string Name { get; private set; }

        public CreateChatRoomCommand(string name)
        {
            Name = name;
        }
    }

    public class CreateChatRoomCommandResult : CommandResult
    {
        public string Code { get; set; }
    }

    public sealed class CreateChatRoomCommandHandler : ICommandHandler<CreateChatRoomCommand, CreateChatRoomCommandResult>
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
            var newRoom = new ChatRoom(command.Name);

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
