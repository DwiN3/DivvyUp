using DivvyUp.Web.InterfaceWeb;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DivvyUp.Web.Controllers
{
    [Route(ApiRoute.USER_ROUTES.USER_ROUTE)]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userServiceInternal;

        public UserController(IUserService userServiceInternal)
        {
            _userServiceInternal = userServiceInternal;
        }

        [HttpPost(ApiRoute.USER_ROUTES.REGISTER)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            return await _userServiceInternal.Register(request);
        }

        [HttpPost(ApiRoute.USER_ROUTES.LOGIN)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            return await _userServiceInternal.Login(request);
            
        }

        [Authorize]
        [HttpPut(ApiRoute.USER_ROUTES.EDIT)]
        public async Task<IActionResult> Edit([FromBody] RegisterRequest request)
        {
            return await _userServiceInternal.Edit(User, request);
        }

        [Authorize]
        [HttpDelete(ApiRoute.USER_ROUTES.REMOVE)]
        public async Task<IActionResult> Remove()
        {
            return await _userServiceInternal.Remove(User);
        }

        [HttpGet(ApiRoute.USER_ROUTES.VALIDATE_TOKEN)]
        public async Task<IActionResult> ValidateToken([FromRoute] string token)
        {
            return await _userServiceInternal.ValidToken(token);
        }

        [Authorize]
        [HttpGet(ApiRoute.USER_ROUTES.ME)]
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
