using DivvyUp.Web.InterfaceWeb;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DivvyUp.Web.Controllers
{
    [Route("rm/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userServiceInternal;

        public UserController(IUserService userServiceInternal)
        {
            _userServiceInternal = userServiceInternal;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            return await _userServiceInternal.Register(request);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            return await _userServiceInternal.Login(request);
            
        }

        [Authorize]
        [HttpPut("edit")]
        public async Task<IActionResult> Edit([FromBody] RegisterRequest request)
        {
            return await _userServiceInternal.Edit(User, request);
        }

        [Authorize]
        [HttpDelete("remove")]
        public async Task<IActionResult> Remove()
        {
            return await _userServiceInternal.Remove(User);
        }

        [HttpGet("validate-token")]
        public async Task<IActionResult> ValidateToken([FromQuery] string token)
        {
            return await _userServiceInternal.ValidToken(token);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetUser()
        {
            return await _userServiceInternal.GetUser(User);
        }

        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            return await _userServiceInternal.ChangePassword(User, request);
        }
    }
}
