﻿using DivvyUp.Web.InterfaceWeb;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DivvyUp.Web.Controllers
{
    [Route("rm/user/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userServiceInternal;

        public UserController(IUserService userServiceInternal)
        {
            _userServiceInternal = userServiceInternal;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            return await _userServiceInternal.Register(request);
        }

        [HttpPost]
        [Route("auth")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            return await _userServiceInternal.Login(request);
            
        }

        [HttpPut]
        [Route("edit")]
        [Authorize]
        public async Task<IActionResult> Edit([FromBody] RegisterRequest request)
        {
            return await _userServiceInternal.Edit(request, User);
        }

        [HttpDelete]
        [Route("remove")]
        [Authorize]
        public async Task<IActionResult> Remove()
        {
            return await _userServiceInternal.Remove(User);
        }

        [HttpGet]
        [Route("validate-token")]
        public async Task<IActionResult> ValidateToken([FromRoute] string token)
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
