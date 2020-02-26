using System.Threading.Tasks;
using Chatter.Model;
using Chatter.Services;
using Chatter.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chatter.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("{otherUserId}")]
        public async Task<IActionResult> GetMessages(int otherUserId)
        {
            var userId = this.GetUserIdFromAuth();
            var messages = await _messageService.GetMessages(userId, otherUserId);
            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(MessageModel messageModel)
        {
            var userId = this.GetUserIdFromAuth();
            var message = await _messageService.SendMessage(userId, messageModel);
            return Ok(message);
        }
    }
}