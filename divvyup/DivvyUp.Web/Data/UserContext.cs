using DivvyUp_Impl_Maui.Api.Exceptions;
using DivvyUp_Shared.Model;
using System.Net;
using System.Security.Claims;

namespace DivvyUp.Web.Data
{
    public class UserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MyDbContext _dbContext;

        public UserContext(IHttpContextAccessor httpContextAccessor, MyDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public ClaimsPrincipal User => _httpContextAccessor.HttpContext.User;

        private User _currentUser;

        public async Task<User> GetCurrentUser()
        {
            if (_currentUser == null)
            {
                var userIdClaim = User.FindFirst("Id")?.Value;
                if (userIdClaim == null)
                    throw new DException(HttpStatusCode.Unauthorized, "Błędny token");

                var userId = int.Parse(userIdClaim);
                var user = await _dbContext.Users.FindAsync(userId);
                if (user == null)
                    throw new DException(HttpStatusCode.NotFound, "Nie znaleziono osoby");

                _currentUser = user;
            }
            return _currentUser;
        }

        public async Task<int> GetCurrentUserId()
        {
            var user = await GetCurrentUser();
            return user.Id;
        }
    }
}
