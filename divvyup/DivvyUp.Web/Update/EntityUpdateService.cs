using DivvyUp.Web.Data;
using DivvyUp_Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace DivvyUp.Web.Update
{
    public class EntityUpdateService
    {
        private readonly DivvyUpDBContext _dbContext;

        public EntityUpdateService(DivvyUpDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UpdatePerson(User user, bool updateBalance)
        {
            var persons = await _dbContext.Persons
                .Where(p => p.UserId == user.Id)
                .Include(p => p.PersonProducts)
                .ThenInclude(pp => pp.Product)
                .ToListAsync();

            foreach (var person in persons)
            {
                var personProducts = person.PersonProducts;
                var receipts = personProducts
                    .Select(pp => pp.Product.ReceiptId)
                    .Distinct()
                    .ToList();

                person.ReceiptsCount = receipts.Count;
                person.ProductsCount = personProducts.Count;
                person.TotalAmount = personProducts.Sum(pp => pp.PartOfPrice + (pp.Compensation ? pp.Product.CompensationPrice : 0));
                person.UnpaidAmount = personProducts.Where(pp => !pp.Settled).Sum(pp => pp.PartOfPrice + (pp.Compensation ? pp.Product.CompensationPrice : 0));

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
            return await _dbContext.Loans
                .Where(l => l.PersonId == person.Id && !l.Settled)
                .SumAsync(l => l.Lent ? l.Amount : -l.Amount);
        }


        public async Task UpdateTotalPriceReceipt(Receipt receipt)
        {
            receipt.TotalPrice = await _dbContext.Products
                .Where(p => p.ReceiptId == receipt.Id)
                .SumAsync(p => p.Price);

            _dbContext.Receipts.Update(receipt);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> AreAllProductsSettled(Receipt receipt)
        {
            var products = await _dbContext.Products
                .Where(p => p.Receipt.Id == receipt.Id)
                .ToListAsync();
            return products.All(p => p.Settled);
        }

        public async Task UpdateProductDetails(Product product)
        {
            var personProducts = await _dbContext.PersonProducts
                .Where(pp => pp.ProductId == product.Id)
                .ToListAsync();

            var compensationPrice = product.Price - personProducts.Sum(pp => pp.PartOfPrice);
            product.CompensationPrice = compensationPrice;

            var availableQuantity = product.MaxQuantity - personProducts.Sum(pp => pp.Quantity);
            product.AvailableQuantity = availableQuantity;

            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<bool> AreAllPersonProductsSettled(Product product)
        {
            var personProducts = await _dbContext.PersonProducts
                .Where(pp => pp.Product.Id == product.Id)
                .ToListAsync();
            return personProducts.All(pp => pp.Settled);
        }

        public async Task UpdatePartPricesPersonProduct(Product product)
        {
            var personProducts = await _dbContext.PersonProducts
                .Where(pp => pp.ProductId == product.Id)
                .ToListAsync();

            bool isUpdated = false;

            foreach (var personProduct in personProducts)
            {
                var newPartOfPrice = await CalculatePartPrice(personProduct.Quantity, product.MaxQuantity, product.Price);
                if (personProduct.PartOfPrice != newPartOfPrice)
                {
                    personProduct.PartOfPrice = newPartOfPrice;
                    isUpdated = true;
                }
            }

            if (isUpdated)
            {
                _dbContext.PersonProducts.UpdateRange(personProducts);
                await _dbContext.SaveChangesAsync();
            }
        }

        private Task<decimal> CalculatePartPrice(int quantity, int maxQuantity, decimal price)
        {
            return Task.FromResult(((decimal)quantity / maxQuantity) * price);
        }
    }
}
