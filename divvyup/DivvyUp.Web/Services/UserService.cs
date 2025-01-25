using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using DivvyUp.Web.Data;
using DivvyUp.Web.Validation;
using AutoMapper;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;
using DivvyUp_Shared.Interfaces;
using DivvyUp_Shared.Models;
using DivvyUp.Web.EntityManager;

namespace DivvyUp.Web.Services
{
    public class UserService : IUserService
    {
        private readonly IDivvyUpDBContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly EntityManagementService _managementService;
        private readonly DValidator _validator;
        private readonly IMapper _mapper;

        public UserService(IDivvyUpDBContext dbContext, IConfiguration configuration, EntityManagementService managementService, DValidator validator, IMapper mapper)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _managementService = managementService;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task Register(RegisterUserDto request)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsEmpty(request.Username, "Nazwa użytkownika jest wymagana");
            _validator.IsEmpty(request.Name, "Imie jest wymagana");
            _validator.IsEmpty(request.Username, "Nazwa użytkownika jest wymagana");
            _validator.IsEmpty(request.Email, "Email użytkownika jest wymagana");
            _validator.IsEmpty(request.Password, "Hasło jest wymagane");

            if (_dbContext.Users.Any(x => x.Email == request.Email || x.Username == request.Username))
                throw new DException(HttpStatusCode.Conflict, "Użytkownik o takich danych istnieje");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new User
            {
                Username = request.Username,
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                Password = hashedPassword
            };
            _dbContext.Users.Add(newUser);
            await AddPersonUser(newUser);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> Login(LoginUserDto request)
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

        public async Task<string> Edit(EditUserDto request)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsEmpty(request.Username, "Nazwa użytkownika jest wymagana");
            _validator.IsEmpty(request.Email, "Email użytkownika jest wymagana");

            var user = await _managementService.GetUser();

            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(p => (p.Username == request.Username || p.Email == request.Email) && p.Id != user.Id);
            if (existingUser != null)
            {
                throw new DException(HttpStatusCode.Conflict, "Użytkownik o takich danych istnieje");
            }

            if (!user.Name.Equals(request.Name) || !user.Surname.Equals(request.Surname))
            {
                var person = await _dbContext.Persons.FirstOrDefaultAsync(p => p.UserId == user.Id && p.UserAccount);
                person.Name = request.Name;
                person.Surname = request.Surname;
                _dbContext.Persons.Update(person);
            }

            user.Username = request.Username;
            user.Name = request.Name;
            user.Surname = request.Surname;
            user.Email = request.Email;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            var token =  GenerateToken(user);
            return token;
        }

        public async Task Remove()
        {
            var user = await _managementService.GetUser();

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
            var user = await _managementService.GetUser();
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task ChangePassword(ChangePasswordUserDto request)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsEmpty(request.Password, "Hasło jest wymagane");
            _validator.IsEmpty(request.NewPassword, "Nowe hasło jest wymagane");

            var user = await _managementService.GetUser();

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
                Name = user.Name,
                Surname = user.Surname,
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
