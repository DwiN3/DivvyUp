using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DivvyUp.Web.RequestDto;

namespace DivvyUp.Web.Interface
{
    public interface IUserServiceB
    {
        Task<IActionResult> Register(RegisterRequest request);
        Task<IActionResult> Login(LoginRequest request);
        Task<IActionResult> Edit(RegisterRequest request, ClaimsPrincipal user);
        Task<IActionResult> Remove(string userId, ClaimsPrincipal user);
        Task<IActionResult> ValidToken(string token);
        Task<IActionResult> GetUser(ClaimsPrincipal user);
        Task<IActionResult> ChangePassword(ChangePasswordRequest request, ClaimsPrincipal user);

    }
}
