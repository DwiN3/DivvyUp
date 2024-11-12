using DivvyUp.Web.Validation;
using DivvyUp_Shared.Model;
using System.Security.Claims;

namespace DivvyUp.Web.Data
{
    public class UserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MyValidator _validator;

        public UserContext(IHttpContextAccessor httpContextAccessor, MyValidator validator)
        {
            _httpContextAccessor = httpContextAccessor;
            _validator = validator;
        }

        public ClaimsPrincipal User => _httpContextAccessor.HttpContext.User;

        private User _currentUser;

        public async Task<User> GetCurrentUser()
        {
            if (_currentUser == null)
            {
                _currentUser = await _validator.GetUser(User);
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
