using ChatApp.Application.Chat;
using ChatApp.Domain;
using ChatApp.Domain.Chat.Request;
using ChatApp.Domain.Chat.Response;
using ChatApp.Application.Command;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ICommandDispatcher _dispatcher;

        public ChatController(
            ICommandDispatcher dispatcher
            )
        {
            _dispatcher = dispatcher;
        }

        [HttpPost]
        public async Task<ActionResult<ResultResponse<CreateChatRoomResponse>>> CreateChatRoomAsync(CreateChatRoomRequest request)
        {
            var command = new CreateChatRoomCommand(request.Name);

            var result = await _dispatcher.Dispatch(command);

            return Ok(result);
        }

        [HttpPost("message")]
        public async Task<ActionResult<ResultResponse<object>>> SendMessageAsync(SendMessageRequest request)
        {
            var command = new SendMessageCommand(request.RoomId, request.Message, request.UserName);
            var result = await _dispatcher.Dispatch(command);
            return Ok(result);
        }
    }
}
