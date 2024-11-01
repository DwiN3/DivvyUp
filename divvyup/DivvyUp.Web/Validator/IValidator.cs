using System.Security.Claims;
using DivvyUp_Shared.Model;

namespace DivvyUp.Web.Validator
{
    public interface IValidator
    {
        Task<User> GetUser(ClaimsPrincipal claims);
        Task<Person> GetPerson(ClaimsPrincipal claims, int personId);
    }
}
