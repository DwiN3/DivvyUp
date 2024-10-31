using DivvyUp.Web.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DivvyUp.Web.RequestDto;

namespace DivvyUp.Web.Controllers
{
    [Route("rm/user/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServiceB _userServiceInternal;

        public UserController(IUserServiceB userServiceInternal)
        {
            _userServiceInternal = userServiceInternal;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            return await _userServiceInternal.Register(request);
        }

        [HttpPost]
        [Route("auth")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            return await _userServiceInternal.Login(request);
            
        }

        [HttpPut]
        [Route("edit")]
        [Authorize]
        public async Task<IActionResult> Edit([FromQuery]  [FromBody] RegisterRequest request)
        {
            return await _userServiceInternal.Edit(request, User);
        }

        [HttpDelete]
        [Route("remove/{userId}")]
        [Authorize]
        public async Task<IActionResult> Remove(string userId)
        {
            return await _userServiceInternal.Remove(userId, User);
        }

        [HttpGet]
        [Route("validate-token")]
        public async Task<IActionResult> ValidateToken([FromQuery] string token)
        {
            return await _userServiceInternal.ValidToken(token);
        }

        [HttpGet]
        [Route("get-user")]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            return await _userServiceInternal.GetUser(User);
        }

        [HttpPost]
        [Route("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            return await _userServiceInternal.ChangePassword(request, User);
        }
    }
}
