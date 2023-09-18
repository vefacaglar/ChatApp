using ChatApp.Domain.Database.ChatDb;
using ChatApp.Domain.Database.ChatDb.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ChatController : ControllerBase
    {
        public ChatController(
            )
        {
        }

        [HttpGet]
        public async Task<ActionResult> Test()
        {
            return Ok();
        }
    }
}
