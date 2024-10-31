using System.Security.Claims;
using Azure.Core;
using DivvyUp.Web.Interface;
using DivvyUp.Web.Models;
using DivvyUp.Web.RequestDto;
using DivvyUp_Shared.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DivvyUp.Web.Service
{
    public class PersonService : IPersonService
    {
        private readonly MyDbContext _dbContext;

        public PersonService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Add(AddEditPersonRequest person, ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst("UserId")?.Value;
            if (userIdClaim == null)
                return new UnauthorizedObjectResult("Błędny token");

            var userId = int.Parse(userIdClaim);
            var userEntity = await _dbContext.Users.FindAsync(userId);
            if (userEntity == null)
                return new NotFoundObjectResult("Nie znaleziono użytkownika");


            var newPerson = new Person()
            {
                 User = userEntity,
                 Name = person.Name,
                 Surname = person.Surname,
                 ReceiptsCount = 0,
                 ProductsCount = 0,
                 TotalAmount = 0,
                 UnpaidAmount = 0,
                 LoanBalance = 0,
                 UserAccount = false
            };

            _dbContext.People.Add(newPerson);
            await _dbContext.SaveChangesAsync();
            return new OkObjectResult("Pomyślnie dodano osobe");
        }

        public async Task<IActionResult> Edit(AddEditPersonRequest person, int personId, ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Remove(int personId, ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> GetPerson(int personId, ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst("UserId")?.Value;
            if (userIdClaim == null)
                return new UnauthorizedObjectResult("Błędny token");

            var userId = int.Parse(userIdClaim);
            var userEntity = await _dbContext.Users.FindAsync(userId);
            if (userEntity == null)
                return new NotFoundObjectResult("Nie znaleziono użytkownika");

            var personEntity = await _dbContext.People.FindAsync(personId);

            if (personEntity == null)
                return new NotFoundObjectResult("Nie znaleziono osoby");

            var personDto = new PersonDto()
            {
                id = personEntity.Id,
                name = personEntity.Name,
                surname = personEntity.Surname,
                loanBalance = personEntity.LoanBalance,
                productsCount = personEntity.ProductsCount,
                receiptsCount = personEntity.ReceiptsCount,
                totalAmount = personEntity.TotalAmount,
                unpaidAmount = personEntity.UnpaidAmount,
                userAccount = personEntity.UserAccount

            };

            return new OkObjectResult(personDto);
        }

        public Task<IActionResult> GetPersons(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetUserPerson(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetPersonFromReceipt(int receiptId, ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetPersonFromProduct(int productId, ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }
    }
}
