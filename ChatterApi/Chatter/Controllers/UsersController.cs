using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chatter.Model;
using Chatter.Services;
using Chatter.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chatter.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            var token = await _userService.RegisterUser(registerModel);
            return string.IsNullOrWhiteSpace(token)
                ? (IActionResult) BadRequest()
                : Ok(new { Token = token });
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var token = await _userService.Login(loginModel);
            return string.IsNullOrWhiteSpace(token)
                ? (IActionResult)BadRequest()
                : Ok(new {Token = token});
        }

        [HttpGet("Friends")]
        public async Task<IActionResult> GetFriends()
        {
            var userId = this.GetUserIdFromAuth();
            var friends = await _userService.GetFriends(userId);
            return Ok(friends);
        }

        [HttpPut("Friends/{username}")]
        public async Task<IActionResult> AddFriend(string username)
        {
            var userId = this.GetUserIdFromAuth();
            await _userService.AddFriend(userId, username);
            return Ok();
        }
    }
}