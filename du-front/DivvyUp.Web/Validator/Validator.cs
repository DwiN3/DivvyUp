using System.Net;
using DivvyUp.Web.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace DivvyUp.Web.Validator
{
    public class Validator : IValidator
    {
        private readonly MyDbContext _dbContext;

        public Validator(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUser(ClaimsPrincipal claims)
        {
            var userIdClaim = claims.FindFirst("UserId")?.Value;
            if (userIdClaim == null)
                throw new ValidException(HttpStatusCode.Unauthorized,"Błędny token");

            var userId = int.Parse(userIdClaim);
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                throw new ValidException(HttpStatusCode.NotFound,"Nie znaleziono osoby");

            return user;
        }

        public async Task<Person> GetPerson(ClaimsPrincipal claims, int personId)
        {
            var user = await GetUser(claims);

            var person = await _dbContext.People
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == personId);
            if (person == null)
                throw new ValidException(HttpStatusCode.NotFound,"Osoba nie znaleziona");
            if (person.User.UserId != user.UserId)
                throw new ValidException(HttpStatusCode.Unauthorized,"Brak dostępu do osoby: "+person.Id);

            return person;
        }
    }
}
