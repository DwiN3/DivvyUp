using System.Security.Claims;
using DivvyUp.Web.Validator;
using DivvyUp_Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace DivvyUp.Web.Update
{
    public class EntityUpdateService : IEntityUpdateService
    {
        private readonly MyDbContext _dbContext;
        private readonly IValidator _validator;

        public EntityUpdateService(MyDbContext dbContext, IValidator validator)
        {
            _dbContext = dbContext;
            _validator = validator;
        }

        public async Task UpdatePerson(ClaimsPrincipal claims, bool updateBalance)
        {
            var user = await _validator.GetUser(claims);
            var persons = await _dbContext.Persons.Where(p => p.User == user).ToListAsync();

            foreach (var person in persons)
            {
                var personProducts = await _dbContext.PersonProducts
                    .Where(pp => pp.Person.Id == person.Id)
                    .Include(pp => pp.Product)
                    .ToListAsync();

                person.ReceiptsCount = personProducts
                    .Where(pp => pp.Product.Receipt != null)
                    .Select(pp => pp.Product.Receipt.Id)
                    .Distinct()
                    .Count();

                person.ProductsCount = personProducts.Count;
                person.TotalAmount = personProducts.Sum(pp => pp.PartOfPrice);
                person.UnpaidAmount = personProducts.Where(pp => !pp.Settled).Sum(pp => pp.PartOfPrice);

                if (updateBalance)
                {
                    person.LoanBalance = await CalculateBalance(person);
                }
            }

            _dbContext.Persons.UpdateRange(persons);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<decimal> CalculateBalance(Person person)
        {
            var loans = await _dbContext.Loans.Where(l => l.Person.Id == person.Id).ToListAsync();
            decimal balance = 0;

            foreach (var loan in loans)
            {
                if (!loan.Settled)
                {
                    balance += loan.Lent ? loan.Amount : -loan.Amount;
                }
            }

            return balance;
        }

        public async Task UpdateTotalPriceReceipt(Receipt receipt)
        {
            var products = await _dbContext.Products.Where(p => p.Receipt.Id == receipt.Id).ToListAsync();
            receipt.TotalPrice = products.Sum(p => p.Price);

            _dbContext.Receipts.Update(receipt);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> AreAllProductsSettled(Receipt receipt)
        {
            var products = await _dbContext.Products.Where(p => p.Receipt.Id == receipt.Id).ToListAsync();
            return products.All(p => p.Settled);
        }

        public async Task UpdateCompensationPrice(Product product)
        {
            var personProducts = await _dbContext.PersonProducts.Where(pp => pp.Product.Id == product.Id).ToListAsync();
            product.CompensationPrice = await CalculateCompensationPrice(personProducts, product.Price);

            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
        }

        private Task<decimal> CalculateCompensationPrice(List<PersonProduct> personProducts, decimal price)
        {
            var compensationPrice = price - personProducts.Sum(pp => pp.PartOfPrice);
            return Task.FromResult(compensationPrice);
        }

        public async Task<bool> AreAllPersonProductsSettled(Product product)
        {
            var personProducts = await _dbContext.PersonProducts.Where(pp => pp.Product.Id == product.Id).ToListAsync();
            return personProducts.All(pp => pp.Settled);
        }

        public async Task UpdatePartPricesPersonProduct(Product product)
        {
            var personProducts = await _dbContext.PersonProducts.Where(pp => pp.Product.Id == product.Id).ToListAsync();

            foreach (var personProduct in personProducts)
            {
                personProduct.PartOfPrice =
                    await CalculatePartPrice(personProduct.Quantity, product.MaxQuantity, product.Price);
                _dbContext.PersonProducts.Update(personProduct);
            }

            await _dbContext.SaveChangesAsync();
        }

        private Task<decimal> CalculatePartPrice(int quantity, int maxQuantity, decimal price)
        {
            return Task.FromResult(((decimal)quantity / maxQuantity) * price);
        }
    }
}
