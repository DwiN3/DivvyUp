using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DivvyUp.Web.InterfaceWeb;
using Microsoft.AspNetCore.Mvc;
using DivvyUp.Web.Validator;
using DivvyUp_Shared.Model;
using DivvyUp_Shared.RequestDto;
using System.ComponentModel.DataAnnotations;
using Azure.Core;
using System.Linq.Dynamic.Core.Tokenizer;

namespace DivvyUp.Web.Service
{
    public class UserService : IUserService
    {
        private readonly MyDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IValidator _validator;

        public UserService(MyDbContext dbContext, IConfiguration configuration, IValidator validator)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _validator = validator;
        }

        public async Task<IActionResult> Register(RegisterRequest request)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsEmpty(request.Username, "Nazwa użytkownika jest wymagana");
            _validator.IsEmpty(request.Email, "Email użytkownika jest wymagana");
            _validator.IsEmpty(request.Password, "Hasło jest wymagane");

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
            await AddPersonUser(newUser);

            await _dbContext.SaveChangesAsync();

            return new OkObjectResult("Pomyślnie zarejstrowano");
        }

        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                _validator.IsNull(request, "Nie przekazano danych");
                _validator.IsEmpty(request.Username, "Nazwa użytkownika jest wymagana");
                _validator.IsEmpty(request.Password, "Hasło jest wymagane");

                var user = _dbContext.Users.FirstOrDefault(x => x.Username == request.Username);
                if (user == null)
                    return new NotFoundObjectResult("Nie znaleziono użytkownika");
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                    return new UnauthorizedObjectResult("Błędne hasło");

                var token = GenerateToken(user);
                return new OkObjectResult(token);
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> Edit(ClaimsPrincipal claims, RegisterRequest request)
        {
            try
            {
                _validator.IsNull(request, "Nie przekazano danych");
                _validator.IsEmpty(request.Username, "Nazwa użytkownika jest wymagana");
                _validator.IsEmpty(request.Email, "Email użytkownika jest wymagana");

                var user = await _validator.GetUser(claims);
                user.Username = request.Username;
                user.Email = request.Email;

                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
                var token =  GenerateToken(user);

                return new OkObjectResult(token);
            }
            catch (ValidException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }


        public async Task<IActionResult> Remove(ClaimsPrincipal claims)
        {
            try
            {
                var user = await _validator.GetUser(claims);
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
                return new OkObjectResult("Usunięto użytkownika");
            }
            catch (ValidException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }


        public Task<IActionResult> ValidToken(string token)
        {
            _validator.IsEmpty(token, "Token jest wymagany");

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


        public async Task<IActionResult> GetUser(ClaimsPrincipal claims)
        {
            try
            {
                var user = await _validator.GetUser(claims);
                return new OkObjectResult(user);
            }
            catch (ValidException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> ChangePassword(ClaimsPrincipal claims, ChangePasswordRequest request)
        {
            try
            {
                _validator.IsNull(request, "Nie przekazano danych");
                _validator.IsEmpty(request.Password, "Hasło jest wymagane");
                _validator.IsEmpty(request.NewPassword, "Nowe hasło jest wymagane");

                var user = await _validator.GetUser(claims);

                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                    return new UnauthorizedObjectResult("Błędne hasło");

                user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
                return new OkObjectResult("Pomyślnie zmieniono hasło");
            }
            catch (ValidException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        private async Task AddPersonUser(User user)
        {
            var newPerson = new Person()
            {
                User = user,
                Name = user.Username,
                Surname = string.Empty,
                ReceiptsCount = 0,
                ProductsCount = 0,
                TotalAmount = 0,
                UnpaidAmount = 0,
                LoanBalance = 0,
                UserAccount = true
            };

            _dbContext.Persons.Add(newPerson);
        }

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Id", user.Id.ToString()),
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
