using DivvyUp.Web.Interface;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace DivvyUp.Web.Controllers
{
    [ApiController]
    [SwaggerTag("User Management")]
    [Route(ApiRoute.USER_ROUTES.USER_ROUTE)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userServiceInternal;

        public UserController(IUserService userServiceInternal)
        {
            _userServiceInternal = userServiceInternal;
        }

        [HttpPost(ApiRoute.USER_ROUTES.REGISTER)]
        [SwaggerOperation(Summary = "Register a new user", Description = "Registers a new user account in the system.")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            return await _userServiceInternal.Register(request);
        }

        [HttpPost(ApiRoute.USER_ROUTES.LOGIN)]
        [SwaggerOperation(Summary = "Authenticate user", Description = "Authenticates a user and returns an authentication token.")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            return await _userServiceInternal.Login(request);
        }

        [Authorize]
        [HttpPut(ApiRoute.USER_ROUTES.EDIT)]
        [SwaggerOperation(Summary = "Edit user account", Description = "Edits the details of the currently authenticated user.")]
        public async Task<IActionResult> Edit([FromBody] RegisterRequest request)
        {
            return await _userServiceInternal.Edit(User, request);
        }

        [Authorize]
        [HttpDelete(ApiRoute.USER_ROUTES.REMOVE)]
        [SwaggerOperation(Summary = "Remove user account", Description = "Removes the currently authenticated user's account.")]
        public async Task<IActionResult> Remove()
        {
            return await _userServiceInternal.Remove(User);
        }

        [HttpGet(ApiRoute.USER_ROUTES.VALIDATE_TOKEN)]
        [SwaggerOperation(Summary = "Validate token", Description = "Checks the validity of a given authentication token.")]
        public async Task<IActionResult> ValidateToken([FromRoute] string token)
        {
            return await _userServiceInternal.ValidToken(token);
        }

        [Authorize]
        [HttpGet(ApiRoute.USER_ROUTES.ME)]
        [SwaggerOperation(Summary = "Retrieve user by token", Description = "Retrieves the user information associated with the provided token.")]
        public async Task<IActionResult> GetUser()
        {
            return await _userServiceInternal.GetUser(User);
        }

        [Authorize]
        [HttpPut("change-password")]
        [SwaggerOperation(Summary = "Change user password", Description = "Changes the password for the currently authenticated user.")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            return await _userServiceInternal.ChangePassword(User, request);
        }
    }
}
