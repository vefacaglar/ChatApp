using ChatApp.Domain;
using ChatApp.Domain.Chat.Request;
using ChatApp.Domain.Chat.Response;
using ChatApp.Domain.Database.ChatDb.Entities;
using ChatApp.Infrastructure;
using MediatR;

namespace ChatApp.Application.Chat
{
    public class CreateChatRoomCommand : IRequest<ResultResponse<CreateChatRoomResponse>>
    {
        public string Name { get; set; }

        public CreateChatRoomCommand(CreateChatRoomRequest request)
        {
            Name = request.Name;
        }

        internal sealed class CreateChatRoomCommandHandler : IRequestHandler<CreateChatRoomCommand, ResultResponse<CreateChatRoomResponse>>
        {
            private readonly IUnitOfWork _uow;

            public CreateChatRoomCommandHandler(
                IUnitOfWork uow
                )
            {
                _uow = uow;
            }

            public async Task<ResultResponse<CreateChatRoomResponse>> Handle(CreateChatRoomCommand request, CancellationToken cancellationToken)
            {
                var newRoom = new ChatRoom()
                {
                    Name = request.Name,
                    CreatedAt = DateTime.UtcNow,
                };

                var repository = _uow.GetRepository<ChatRoom>();
                repository.Add(newRoom);

                await _uow.SaveChangesAsync();

                return new ResultResponse<CreateChatRoomResponse>(new CreateChatRoomResponse
                {
                    Code = newRoom.Id.ToString(),
                });
            }
        }
    }
}
