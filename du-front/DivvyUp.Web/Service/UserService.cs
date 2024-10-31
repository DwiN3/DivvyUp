using DivvyUp.Web.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DivvyUp.Web.Interface;
using DivvyUp.Web.RequestDto;
using Microsoft.AspNetCore.Mvc;

namespace DivvyUp.Web.Service
{
    public class UserService : IUserServiceB
    {
        private readonly MyDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public UserService(MyDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (_dbContext.Users.Any(x => x.Email == request.Email || x.Username == request.Username))
                return new ConflictObjectResult("Użytkownik o takich danych istnieje");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                Password = hashedPassword
            };
            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return new OkObjectResult("Pomyślnie zarejstrowano");
        }

        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Username == request.Username);
            if (user == null)
                return new NotFoundObjectResult("Nie znaleziono użytkownika");
            if(!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                return new UnauthorizedObjectResult("Błędne hasło");

            var token = GenerateToken(user);
            return new OkObjectResult(token);
        }

        public async Task<IActionResult> Edit(RegisterRequest request, ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst("UserId")?.Value;
            if (userIdClaim == null)
                return new UnauthorizedObjectResult("Błędny token");

            var userToUpdate = await _dbContext.Users.FindAsync(userIdClaim);
            if (userToUpdate == null)
                return new NotFoundObjectResult("Nie znaleziono użytkownika");

            userToUpdate.Username = request.Username;
            userToUpdate.Email = request.Email;

            _dbContext.Users.Update(userToUpdate);
            await _dbContext.SaveChangesAsync();

            return new OkObjectResult("Pomyślnie wprowadzono zmiany");
        }


        public async Task<IActionResult> Remove(string userId, ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst("UserId")?.Value;
            if (userIdClaim == null || userId != userIdClaim)
                return new UnauthorizedObjectResult("Błędny token");

            var userToRemove = await _dbContext.Users.FindAsync(userId);
            if (userToRemove == null)
                return new NotFoundObjectResult("Nie znaleziono użytkownika");

            _dbContext.Users.Remove(userToRemove);
            await _dbContext.SaveChangesAsync();

            return new OkObjectResult("Usunięto użytkownika");
        }


        public Task<IActionResult> ValidToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return Task.FromResult<IActionResult>(new OkObjectResult(true));
            }
            catch
            {
                return Task.FromResult<IActionResult>(new OkObjectResult(false));
            }
        }


        public async Task<IActionResult> GetUser(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst("UserId")?.Value;
            if (userIdClaim == null)
                return new UnauthorizedObjectResult("Błędny token");

            var userId = int.Parse(userIdClaim);
            var userEntity = await _dbContext.Users.FindAsync(userId);
            if (userEntity == null)
                return new NotFoundObjectResult("Nie znaleziono użytkownika");

            return new OkObjectResult(userEntity);
        }
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request, ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst("UserId")?.Value;
            if (userIdClaim == null)
                return new UnauthorizedObjectResult("Błędny token");

            var userId = int.Parse(userIdClaim);
            var userEntity = await _dbContext.Users.FindAsync(userId);
            if (userEntity == null)
                return new NotFoundObjectResult("Nie znaleziono użytkownika");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, userEntity.Password))
                return new UnauthorizedObjectResult("Błędne hasło");

            userEntity.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            _dbContext.Users.Update(userEntity);
            await _dbContext.SaveChangesAsync();

            return new OkObjectResult("Pomyślnie zmieniono hasło");
        }

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", user.UserId.ToString()),
                new Claim("Email", user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(2),
                signingCredentials: signIn
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
