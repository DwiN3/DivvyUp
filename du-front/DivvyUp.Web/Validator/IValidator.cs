using System.Security.Claims;
using DivvyUp.Web.Models;

namespace DivvyUp.Web.Validator
{
    public interface IValidator
    {
        Task<User> GetUser(ClaimsPrincipal claims);
        Task<Person> GetPerson(ClaimsPrincipal claims, int personId);
    }
}
