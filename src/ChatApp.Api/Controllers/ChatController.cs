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
            var command = new CreateChatRoomCommand()
            {
                Name = request.Name,
            };

            var result = await _dispatcher.Dispatch(command);

            return Ok(result);
        }
    }
}
