using DivvyUp.Web.Data;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DivvyUp.Web.EntityManager
{
    public class EntityManagementService
    {
        private readonly DivvyUpDBContext _dbContext;
        private readonly UserContext _userContext;

        public EntityManagementService(DivvyUpDBContext dbContext, UserContext userContext)
        {
            _dbContext = dbContext;
            _userContext = userContext;
        }

        public async Task<User> GetUser()
        {
            var user = await _userContext.GetCurrentUser();
            return user;
        }

        public async Task<Person> GetPerson(User user, int personId)
        {
            var person = await _dbContext.Persons.FirstOrDefaultAsync(p => p.Id == personId);
            if (person == null)
                throw new DException(HttpStatusCode.NotFound, "Osoba nie znaleziona");
            if (person.UserId != user.Id)
                throw new DException(HttpStatusCode.Unauthorized, "Brak dostępu do osoby: " + person.Id);

            return person;
        }

        public async Task<Loan> GetLoan(User user, int loanId)
        {
            var loan = await _dbContext.Loans
                .Include(p => p.Person)
                .FirstOrDefaultAsync(p => p.Id == loanId);
            if (loan == null)
                throw new DException(HttpStatusCode.NotFound, "Pożyczka nie znaleziona");
            if (loan.Person.UserId != user.Id)
                throw new DException(HttpStatusCode.Unauthorized, "Brak dostępu do pożyczki: " + loan.Id);

            return loan;
        }

        public async Task<Receipt> GetReceipt(User user, int receiptId)
        {
            var receipt = await _dbContext.Receipts.FirstOrDefaultAsync(p => p.Id == receiptId);
            if (receipt == null)
                throw new DException(HttpStatusCode.NotFound, "Rachunek nie znaleziony");
            if (receipt.UserId != user.Id)
                throw new DException(HttpStatusCode.Unauthorized, "Brak dostępu do rachunku: " + receipt.Id);

            return receipt;
        }

        public async Task<Product> GetProduct(User user, int productId)
        {
            var product = await _dbContext.Products
                .Include(p => p.Receipt)
                .FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
                throw new DException(HttpStatusCode.NotFound, "Produkt nie znaleziony");
            if (product.Receipt.UserId != user.Id)
                throw new DException(HttpStatusCode.Unauthorized, "Brak dostępu do produktu: " + product.Id);

            return product;
        }

        public async Task<PersonProduct> GetPersonProduct(User user, int personProductId)
        {
            var personProduct = await _dbContext.PersonProducts
                .Include(p => p.Product)
                .Include(p => p.Person)
                .FirstOrDefaultAsync(p => p.Id == personProductId);
            if (personProduct == null)
                throw new DException(HttpStatusCode.NotFound, "Przypis osoby z produktem nie znaleziony");
            if (personProduct.Person.UserId != user.Id)
                throw new DException(HttpStatusCode.Unauthorized, "Brak dostępu do przypisu produktu z osobą: " + personProductId);

            return personProduct;
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
                person.CompensationAmount = personProducts.Sum(pp => pp.Compensation ? pp.Product.CompensationPrice : 0);

                if (updateBalance)
                {
                    if (person.UserAccount)
                    {
                        person.LoanBalance = await _dbContext.Loans
                            .Where(l => !l.Settled)
                            .SumAsync(l => !l.Lent ? - l.Amount : l.Amount);
                    }
                    else
                    {
                        person.LoanBalance = await CalculateBalance(person);
                    }
                }
            }

            _dbContext.Persons.UpdateRange(persons);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<decimal> CalculateBalance(Person person)
        {
            return await _dbContext.Loans
                .Where(l => l.PersonId == person.Id && !l.Settled)
                .SumAsync(l => !l.Lent ? l.Amount : - l.Amount);
        }


        public async Task UpdateTotalPriceReceipt(Receipt receipt)
        {
            receipt.TotalPrice = await _dbContext.Products
                .Where(p => p.ReceiptId == receipt.Id)
                .SumAsync(p => p.TotalPrice);

            _dbContext.Receipts.Update(receipt);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> AreAllProductsSettled(int receiptId)
        {
            var products = await _dbContext.Products
                .Where(p => p.ReceiptId == receiptId)
                .ToListAsync();
            return products.All(p => p.Settled);
        }

        public async Task UpdateProductDetails(Product product)
        {
            var personProducts = await _dbContext.PersonProducts
                .Where(pp => pp.ProductId == product.Id)
                .ToListAsync();

            var compensationPrice = product.TotalPrice - personProducts.Sum(pp => pp.PartOfPrice);
            product.CompensationPrice = compensationPrice;

            var availableQuantity = product.MaxQuantity - personProducts.Sum(pp => pp.Quantity);
            product.AvailableQuantity = availableQuantity;

            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProductDetails(List<Product> products)
        {
            var productIds = products.Select(p => p.Id).ToList();
            var personProducts = await _dbContext.PersonProducts
                .Where(pp => productIds.Contains(pp.ProductId))
                .ToListAsync();

            var productMap = products.ToDictionary(p => p.Id, p => p);

            foreach (var personProduct in personProducts)
            {
                var product = productMap[personProduct.ProductId];

                var compensationPrice = product.TotalPrice - personProducts
                    .Where(pp => pp.ProductId == product.Id)
                    .Sum(pp => pp.PartOfPrice);
                product.CompensationPrice = compensationPrice;

                var availableQuantity = product.MaxQuantity - personProducts
                    .Where(pp => pp.ProductId == product.Id)
                    .Sum(pp => pp.Quantity);
                product.AvailableQuantity = availableQuantity;
            }

            _dbContext.Products.UpdateRange(products);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> AreAllPersonProductsSettled(int productId)
        {
            var personProducts = await _dbContext.PersonProducts
                .Where(pp => pp.ProductId == productId)
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
                var newPartOfPrice = await CalculatePartOfPrice(personProduct.Quantity, product.MaxQuantity, product.TotalPrice);
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

        public async Task UpdatePartPricesPersonProduct(List<Product> products)
        {
            var productIds = products.Select(p => p.Id).ToList();
            var personProducts = await _dbContext.PersonProducts
                .Where(pp => productIds.Contains(pp.ProductId))
                .ToListAsync();

            bool isUpdated = false;

            var productMap = products.ToDictionary(p => p.Id, p => p);

            foreach (var personProduct in personProducts)
            {
                var product = productMap[personProduct.ProductId];
                var newPartOfPrice = await CalculatePartOfPrice(personProduct.Quantity, product.MaxQuantity, product.TotalPrice);

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

        public Task<decimal> CalculatePartOfPrice(int quantity, int maxQuantity, decimal price)
        {
            decimal result = price / maxQuantity * quantity;
            decimal roundedResult = Math.Floor(result * 100) / 100;
            return Task.FromResult(roundedResult);
        }

        public decimal CalculateTotalPrice(decimal price, int purchasedQuantity, decimal additionalPrice, decimal discountPercentage)
        {
            var basePrice = price * purchasedQuantity;

            decimal discountAmount = 0;
            if (discountPercentage != 0)
            {
                discountAmount = basePrice * (discountPercentage / 100);
            }

            var totalPrice = (basePrice - discountAmount) + additionalPrice;
            return totalPrice;
        }

        public async Task<PersonProduct> GetPersonWithLowestCompensation(int productId)
        {
            var personProducts = await _dbContext.PersonProducts
                .Include(p => p.Person)
                .Where(pp => pp.ProductId == productId).ToListAsync();

            if (!personProducts.Any())
            {
                return null;
            }

            var personWithLowestCompensation = personProducts
                .OrderBy(pp => pp.Person.CompensationAmount)
                .FirstOrDefault();

            return personWithLowestCompensation;
        }

        public async Task UpdateCompensationFlags(int productId, PersonProduct selectedPersonProduct)
        {
            var personProducts = await _dbContext.PersonProducts
                .Where(pp => pp.ProductId == productId).ToListAsync();
            if (selectedPersonProduct == null || personProducts == null)
            {
                return;
            }

            foreach (var personProduct in personProducts)
            {
                personProduct.Compensation = personProduct.PersonId == selectedPersonProduct.PersonId;
                _dbContext.PersonProducts.Update(personProduct);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
