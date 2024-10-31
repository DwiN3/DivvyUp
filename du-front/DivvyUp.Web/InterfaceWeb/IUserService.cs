using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DivvyUp_Shared.RequestDto;

namespace DivvyUp.Web.InterfaceWeb
{
    public interface IUserService
    {
        Task<IActionResult> Register(RegisterRequest request);
        Task<IActionResult> Login(LoginRequest request);
        Task<IActionResult> Edit(RegisterRequest request, ClaimsPrincipal user);
        Task<IActionResult> Remove(ClaimsPrincipal user);
        Task<IActionResult> ValidToken(string token);
        Task<IActionResult> GetUser(ClaimsPrincipal user);
        Task<IActionResult> ChangePassword(ChangePasswordRequest request, ClaimsPrincipal user);
    }
}
