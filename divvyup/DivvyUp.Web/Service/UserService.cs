using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using DivvyUp_Shared.Model;
using DivvyUp_Shared.RequestDto;
using Microsoft.EntityFrameworkCore;
using DivvyUp.Web.Data;
using DivvyUp.Web.Validation;
using DivvyUp_Impl_Maui.Api.Exceptions;
using DivvyUp_Shared.Dto;
using AutoMapper;
using DivvyUp_Shared.Interface;

namespace DivvyUp.Web.Service
{
    public class UserService : IUserService
    {
        private readonly MyDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly MyValidator _validator;
        private readonly UserContext _userContext;

        public UserService(MyDbContext dbContext, IMapper mapper, IConfiguration configuration, MyValidator validator, UserContext userContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
            _validator = validator;
            _userContext = userContext;
        }

        public async Task Register(RegisterUserRequest request)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsEmpty(request.Username, "Nazwa użytkownika jest wymagana");
            _validator.IsEmpty(request.Email, "Email użytkownika jest wymagana");
            _validator.IsEmpty(request.Password, "Hasło jest wymagane");

            if (_dbContext.Users.Any(x => x.Email == request.Email || x.Username == request.Username))
                throw new DException(HttpStatusCode.Conflict, "Użytkownik o takich danych istnieje");

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
        }

        public async Task<string> Login(LoginUserRequest request)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsEmpty(request.Username, "Nazwa użytkownika jest wymagana");
            _validator.IsEmpty(request.Password, "Hasło jest wymagane");

            var user = _dbContext.Users.FirstOrDefault(x => x.Username == request.Username);
            if (user == null)
                throw new DException(HttpStatusCode.NotFound,"Nie znaleziono użytkownika");
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                throw new DException(HttpStatusCode.Unauthorized, "Błędne hasło");

            var token = GenerateToken(user);
            return token;
        }

        public async Task<string> Edit(EditUserRequest request)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsEmpty(request.Username, "Nazwa użytkownika jest wymagana");
            _validator.IsEmpty(request.Email, "Email użytkownika jest wymagana");

            var user = await _userContext.GetCurrentUser();

            if (!user.Username.Equals(request.Username))
            {
                var person = await _dbContext.Persons.FirstOrDefaultAsync(p => p.UserId == user.Id && p.UserAccount);
                person.Name = request.Username;
                _dbContext.Persons.Update(person);
                
            }

            user.Username = request.Username;
            user.Email = request.Email;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            var token =  GenerateToken(user);
            return token;
        }


        public async Task Remove()
        {
            var user = await _userContext.GetCurrentUser();

            var persons = await _dbContext.Persons
                .Where(p => p.UserId == user.Id)
                .ToListAsync();

            var personIds = persons.Select(p => p.Id).ToList();

            var loans = await _dbContext.Loans
                .Where(l => personIds.Contains(l.PersonId))
                .ToListAsync();

            var receipts = await _dbContext.Receipts
                .Where(r => r.UserId == user.Id)
                .ToListAsync();

            var receiptIds = receipts.Select(r => r.Id).ToList();

            var products = await _dbContext.Products
                .Where(p => receiptIds.Contains(p.ReceiptId))
                .ToListAsync();

            var personProducts = await _dbContext.PersonProducts
                .Where(pp => personIds.Contains(pp.PersonId))
                .ToListAsync();

            _dbContext.Loans.RemoveRange(loans);
            _dbContext.PersonProducts.RemoveRange(personProducts);
            _dbContext.Products.RemoveRange(products);
            _dbContext.Receipts.RemoveRange(receipts);
            _dbContext.Persons.RemoveRange(persons);
            _dbContext.Users.Remove(user);

            await _dbContext.SaveChangesAsync();
        }


        public async Task<bool> ValidToken(string token)
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

                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<UserDto> GetUser()
        {
            var user = await _userContext.GetCurrentUser();
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task ChangePassword(ChangePasswordUserRequest request)
        {
                _validator.IsNull(request, "Nie przekazano danych");
                _validator.IsEmpty(request.Password, "Hasło jest wymagane");
                _validator.IsEmpty(request.NewPassword, "Nowe hasło jest wymagane");

            var user = await _userContext.GetCurrentUser();

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                    throw new DException(HttpStatusCode.Unauthorized,"Błędne hasło");

                user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
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
