using ChatApp.Application.Command;
using ChatApp.Domain.Entities.Command;
using ChatApp.Infrastructure;
using ChatApp.Infrastructure.Repositories.Abstractions;
using System.Windows.Input;

namespace ChatApp.Application.Chat
{
    public class SendMessageCommand : ICommand<CommandResult>
    {
        public Guid RoomId { get; private set; }
        public string Message { get; private set; }
        public string UserName { get; private set; }
        public SendMessageCommand(
            Guid roomId, string message, string userName
            )
        {
            RoomId = roomId;
            Message = message;
            UserName = userName;
        }
    }

    public sealed class SendMessageCommandHandler : ICommandHandler<SendMessageCommand, CommandResult>
    {
        private readonly IUnitOfWork _uow;
        private readonly IChatRepository _chatRepository;
        private readonly IEventBus _eventBus;

        public SendMessageCommandHandler(
            IUnitOfWork uow,
            IChatRepository chatRepository,
            IEventBus eventBus
            )
        {
            _uow = uow;
            _chatRepository = chatRepository;
            _eventBus = eventBus;
        }

        public async Task<CommandResult> Handle(SendMessageCommand command)
        {
            var room = await _chatRepository.GetByIdWithMessagesAsync(command.RoomId);
            room.AddMessage(command.UserName, command.Message);
            await _uow.SaveChangesAsync();

            return new CommandResult(true);
        }
    }
}
