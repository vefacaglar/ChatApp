using ChatApp.Application.Chat;
using ChatApp.Application.Command;
using ChatApp.Domain.Entities.Command;
using ChatApp.Infrastructure;
using ChatApp.Infrastructure.Repositories.Abstractions;
using Moq;

namespace ChatApp.Test.Command
{
    public class ChatRoomTest
    {
        private readonly CreateChatRoomCommandHandler _handler;
        private readonly Mock<ICommandDispatcher> _command;
        private readonly Mock<IEventBus> _eventBus;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IRepository<ChatRoom>> _chatRoomRepository;

        public ChatRoomTest()
        {
            _eventBus = new Mock<IEventBus>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _command = new Mock<ICommandDispatcher>();
            _chatRoomRepository = new Mock<IRepository<ChatRoom>>();
            _chatRoomRepository.Setup(x => x.Add(It.IsAny<ChatRoom>()));
            _unitOfWork.Setup(x => x.GetRepository<ChatRoom>()).Returns(_chatRoomRepository.Object);

            _handler = new CreateChatRoomCommandHandler(_unitOfWork.Object, _eventBus.Object);
        }

        [Fact]
        public async Task CreateChatRoom_WithName_MustHaveCode()
        {
            var command = new CreateChatRoomCommand("test");

            var result = await _handler.Handle(command);

            Assert.NotEmpty(result.Code);
        }

    }
}
