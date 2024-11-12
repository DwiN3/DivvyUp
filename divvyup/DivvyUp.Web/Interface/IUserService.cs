using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Model;
using DivvyUp_Shared.RequestDto;

namespace DivvyUp.Web.Interface
{
    public interface IUserService
    {
        Task Register(RegisterRequest request);
        Task<string> Login(LoginRequest request);
        Task<string> Edit(RegisterRequest request);
        Task Remove();
        Task<bool> ValidToken(string token);
        Task<User> GetUser();
        Task ChangePassword(ChangePasswordRequest request);
    }
}
