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
}
