using ChatApp.Domain;
using ChatApp.Domain.Chat.Request;
using ChatApp.Domain.Chat.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ChatController : ControllerBase
    {
        ISender _sender;

        public ChatController(
            ISender sender
            )
        {
            _sender = sender;
        }

        [HttpPost]
        public ActionResult<ResultResponse<CreateChatRoomResponse>> CreateChatRoomAsync(CreateChatRoomRequest request)
        {
            return Ok();
        }
    }
}
