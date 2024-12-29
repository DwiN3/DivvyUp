using AutoMapper;
using DivvyUp.Web.Data;
using DivvyUp.Web.EntityManager;
using DivvyUp.Web.Validation;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.Interfaces;
using DivvyUp_Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DivvyUp.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly DivvyUpDBContext _dbContext;
        private readonly EntityManagementService _managementService;
        private readonly DValidator _validator;
        private readonly IMapper _mapper;

        public ProductService(DivvyUpDBContext dbContext, EntityManagementService managementService, DValidator validator, IMapper mapper)
        {
            _dbContext = dbContext;
            _managementService = managementService;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task Add(AddEditProductDto request, int receiptId)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsEmpty(request.Name, "Nazwa produktu jest wymagana");
            _validator.IsNull(request.Price, "Cena jest wymagana");
            _validator.IsMinusValue(request.Price, "Cena nie może być ujemna");
            _validator.IsNull(request.MaxQuantity, "Maksymalna liczba części jest wymagana");
            _validator.IsNull(request.Divisible, "Informacja o podzielności jest wymagana");
            _validator.IsNull(receiptId, "Brak identyfikatora rachunku");
            var user = await _managementService.GetUser();
            var receipt = await _managementService.GetReceipt(user, receiptId);

            if (!request.Divisible && request.MaxQuantity > 1)
            {
                throw new DException(HttpStatusCode.BadRequest, "Maksymalna liczba podzielności produktu musi być równa 1 gdy produkt jest niepodzielny");
            }
            if (request.Divisible && request.MaxQuantity == 1)
            {
                throw new DException(HttpStatusCode.BadRequest, "Maksymalna liczba podzielności produktu musi być większa od 1 gdy produkt jest podzielny");
            }

            var newProduct = new Product()
            {
                Receipt = receipt,
                Name = request.Name,
                Price = request.Price,
                Divisible = request.Divisible,
                MaxQuantity = request.Divisible ? request.MaxQuantity : 1,
                AvailableQuantity = request.Divisible ? request.MaxQuantity : 1,
                CompensationPrice = request.Divisible ? request.Price : 0,
                Settled = false,
            };

            _dbContext.Products.Add(newProduct);
            await _dbContext.SaveChangesAsync();
            await _managementService.UpdateTotalPriceReceipt(receipt);
        }

        public async Task Edit(AddEditProductDto request, int productId)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsEmpty(request.Name, "Nazwa produktu jest wymagana");
            _validator.IsNull(request.Price, "Cena jest wymagana");
            _validator.IsMinusValue(request.Price, "Cena nie może być ujemna");
            _validator.IsNull(request.MaxQuantity, "Maksymalna liczba części jest wymagana");
            _validator.IsNull(request.Divisible, "Informacja o podzielności jest wymagana");
            _validator.IsNull(productId, "Brak identyfikatora produktu");
            var user = await _managementService.GetUser();
            var product = await _managementService.GetProduct(user, productId);

            if (!request.Divisible && request.MaxQuantity > 1)
            {
                throw new DException(HttpStatusCode.BadRequest, "Maksymalna ilość musi być równa 1 gdy produkt jest niepodzielny");
            }
            if (request.Divisible && request.MaxQuantity == 1)
            {
                throw new DException(HttpStatusCode.BadRequest, "Maksymalna ilość musi być większa od 1 gdy produkt jest podzielny");
            }

            bool previousDivisible = product.Divisible;

            if (previousDivisible && !request.Divisible)
            {
                var personProducts = await _dbContext.PersonProducts.Where(pp => pp.ProductId == product.Id).ToListAsync();
                _dbContext.PersonProducts.RemoveRange(personProducts);
            }

            product.Name = request.Name;
            product.Price = request.Price;
            product.Divisible = request.Divisible;
            product.MaxQuantity = request.Divisible ? request.MaxQuantity : 1;
            product.CompensationPrice = request.Divisible ? request.Price : 0;

            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
            await _managementService.UpdatePartPricesPersonProduct(product);
            await _managementService.UpdateProductDetails(product);
            await _managementService.UpdateTotalPriceReceipt(product.Receipt);
            await _managementService.UpdatePerson(user, false);
        }

        public async Task AddWithPerson(AddEditProductDto request, int receiptId, int personId)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsEmpty(request.Name, "Nazwa produktu jest wymagana");
            _validator.IsNull(request.Price, "Cena jest wymagana");
            _validator.IsMinusValue(request.Price, "Cena nie może być ujemna");
            _validator.IsNull(request.MaxQuantity, "Maksymalna ilość jest wymagana");
            _validator.IsNull(request.Divisible, "Informacja o podzielności jest wymagana");
            _validator.IsNull(receiptId, "Brak identyfikatora rachunku");
            var user = await _managementService.GetUser();
            var receipt = await _managementService.GetReceipt(user, receiptId);

            if (!request.Divisible && request.MaxQuantity > 1)
            {
                throw new DException(HttpStatusCode.BadRequest, "Maksymalna ilość musi być równa 1 gdy produkt jest niepodzielny");
            }
            if (request.Divisible && request.MaxQuantity == 1)
            {
                throw new DException(HttpStatusCode.BadRequest, "Maksymalna ilość musi być większa od 1 gdy produkt jest podzielny");
            }

            var newProduct = new Product()
            {
                Receipt = receipt,
                Name = request.Name,
                Price = request.Price,
                Divisible = request.Divisible,
                MaxQuantity = request.Divisible ? request.MaxQuantity : 1,
                AvailableQuantity = request.Divisible ? request.MaxQuantity : 1,
                CompensationPrice = request.Divisible ? request.Price : 0,
                Settled = false,
            };

            _dbContext.Products.Add(newProduct);
            await _dbContext.SaveChangesAsync();

            if (!request.Divisible)
            {
                var product = _dbContext.Products.Where(p => p == newProduct).FirstOrDefault();
                var personProduct = new PersonProduct()
                {
                    PersonId = personId,
                    ProductId = product.Id,
                    PartOfPrice = product.Price,
                    Quantity = 1,
                    Compensation = true,
                    Settled = false,
                };
                _dbContext.PersonProducts.Add(personProduct);
                await _dbContext.SaveChangesAsync();
            }

            await _managementService.UpdateTotalPriceReceipt(receipt);
            await _managementService.UpdatePerson(user, false);
        }

        public async Task AddWithPersons(AddEditProductDto request, int receiptId, List<int> personIds)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsEmpty(request.Name, "Nazwa produktu jest wymagana");
            _validator.IsMinusValue(request.Price, "Cena nie może być ujemna");
            _validator.IsNull(request.Price, "Cena jest wymagana");
            _validator.IsNull(request.MaxQuantity, "Maksymalna ilość jest wymagana");
            _validator.IsNull(request.Divisible, "Informacja o podzielności jest wymagana");
            _validator.IsNull(receiptId, "Brak identyfikatora rachunku");
            var user = await _managementService.GetUser();
            var receipt = await _managementService.GetReceipt(user, receiptId);

            if (!request.Divisible && request.MaxQuantity > 1)
            {
                throw new DException(HttpStatusCode.BadRequest, "Maksymalna ilość musi być równa 1 gdy produkt jest niepodzielny");
            }
            if (request.Divisible && request.MaxQuantity == 1)
            {
                throw new DException(HttpStatusCode.BadRequest, "Maksymalna ilość musi być większa od 1 gdy produkt jest podzielny");
            }

            var newProduct = new Product()
            {
                Receipt = receipt,
                Name = request.Name,
                Price = request.Price,
                Divisible = true,
                MaxQuantity = request.MaxQuantity,
                AvailableQuantity = request.MaxQuantity,
                CompensationPrice = request.Price,
                Settled = false,
            };

            _dbContext.Products.Add(newProduct);
            await _dbContext.SaveChangesAsync();

            var product = _dbContext.Products.Where(p => p == newProduct).FirstOrDefault();
            foreach (var personId in personIds)
            {
                var newPersonProduct = new PersonProduct()
                {
                    PersonId = personId,
                    ProductId = product.Id,
                    Compensation = false,
                    PartOfPrice = await _managementService.CalculatePartOfPrice(1, request.MaxQuantity, request.Price),
                    Quantity = 1,
                    Settled = product.Settled,
                };
                _dbContext.PersonProducts.Add(newPersonProduct);
            }
            await _dbContext.SaveChangesAsync();

            var nexProduct = await _managementService.GetPersonWithLowestCompensation(product.Id);
            await _managementService.UpdateCompensationFlags(product.Id, nexProduct);
            
            await _managementService.UpdatePartPricesPersonProduct(product);
            await _managementService.UpdateProductDetails(product);
            await _managementService.UpdateTotalPriceReceipt(product.Receipt);
            await _managementService.UpdatePerson(user, false); 
        }

        public async Task EditWithPerson(AddEditProductDto request, int productId, int personId)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsEmpty(request.Name, "Nazwa produktu jest wymagana");
            _validator.IsNull(request.Price, "Cena jest wymagana");
            _validator.IsMinusValue(request.Price, "Cena nie może być ujemna");
            _validator.IsNull(request.MaxQuantity, "Maksymalna ilość jest wymagana");
            _validator.IsNull(request.Divisible, "Informacja o podzielności jest wymagana");
            _validator.IsNull(productId, "Brak identyfikatora produktu");
            var user = await _managementService.GetUser();
            var product = await _managementService.GetProduct(user, productId);

            if (!request.Divisible && request.MaxQuantity > 1)
            {
                throw new DException(HttpStatusCode.BadRequest, "Maksymalna ilość musi być równa 1 gdy produkt jest niepodzielny");
            }
            if (request.Divisible && request.MaxQuantity == 1)
            {
                throw new DException(HttpStatusCode.BadRequest, "Maksymalna ilość musi być większa od 1 gdy produkt jest podzielny");
            }

            bool previousDivisible = product.Divisible;

            product.Name = request.Name;
            product.Price = request.Price;
            product.Divisible = request.Divisible;
            product.MaxQuantity = request.Divisible ? request.MaxQuantity : 1;
            product.CompensationPrice = request.Divisible ? request.Price : 0;
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();

            if (previousDivisible && !request.Divisible)
            {
                var personProducts = await _dbContext.PersonProducts.Where(pp => pp.ProductId == product.Id).ToListAsync();
                _dbContext.PersonProducts.RemoveRange(personProducts);
                var personProduct = new PersonProduct()
                {
                    PersonId = personId,
                    ProductId = product.Id,
                    PartOfPrice = product.Price,
                    Quantity = 1,
                    Compensation = true,
                    Settled = false,
                };
                _dbContext.PersonProducts.Add(personProduct);
                await _dbContext.SaveChangesAsync();
            }
            else if (!previousDivisible && request.Divisible)
            {
                var personProducts = await _dbContext.PersonProducts.Where(pp => pp.ProductId == product.Id).ToListAsync();
                _dbContext.PersonProducts.RemoveRange(personProducts);
                await _dbContext.SaveChangesAsync();
            }
            else if (!request.Divisible)
            {
                var personProducts = await _dbContext.PersonProducts
                    .Where(pp => pp.ProductId == product.Id)
                    .ToListAsync();
                var personProduct = personProducts.FirstOrDefault();
                if (personProduct.PersonId != personId)
                {
                    personProduct.PersonId = personId;
                    _dbContext.PersonProducts.Update(personProduct);
                    await _dbContext.SaveChangesAsync();
                }
            }

            await _managementService.UpdatePartPricesPersonProduct(product);
            await _managementService.UpdateProductDetails(product);
            await _managementService.UpdateTotalPriceReceipt(product.Receipt);
            await _managementService.UpdatePerson(user, false);
        }

        public async Task Remove(int productId)
        {
            _validator.IsNull(productId, "Brak identyfikatora produktu");
            var user = await _managementService.GetUser();
            var product = await _managementService.GetProduct(user, productId);
            var receipt = await _managementService.GetReceipt(user, product.ReceiptId);

            var personProducts = await _dbContext.PersonProducts.Where(pp => pp.ProductId == productId).ToListAsync();
            _dbContext.PersonProducts.RemoveRange(personProducts);
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            await _managementService.UpdateTotalPriceReceipt(receipt);
            await _managementService.UpdatePerson(user, false);
        }

        public async Task SetSettled(int productId, bool settled)
        {
            _validator.IsNull(productId, "Brak identyfikatora produktu");
            _validator.IsNull(settled, "Brak decyzji rozliczenia");
            var user = await _managementService.GetUser();

            var product = await _managementService.GetProduct(user, productId);

            product.Settled = settled;
            _dbContext.Products.Update(product);

            var personProducts = await _dbContext.PersonProducts.Where(pp => pp.ProductId == product.Id).ToListAsync();
            foreach (var personProduct in personProducts)
            {
                personProduct.Settled = settled;
                _dbContext.PersonProducts.Update(personProduct);
            }

            var receipt = product.Receipt;
            bool allSettled = await _managementService.AreAllProductsSettled(receipt.Id);
            receipt.Settled = allSettled;
            _dbContext.Receipts.Update(receipt);

            await _dbContext.SaveChangesAsync();
            await _managementService.UpdatePerson(user, false);
        }

        public async Task<ProductDto> GetProduct(int productId)
        {
            _validator.IsNull(productId, "Brak identyfikatora produktu");
            var user = await _managementService.GetUser();
            var product = await _managementService.GetProduct(user, productId);
            var productDto = MapProductToDto(product);
            return productDto;
        }


        public async Task<List<ProductDto>> GetProducts()
        {
            var user = await _managementService.GetUser();
            var products = await _dbContext.Products
                .AsNoTracking()
                .Include(p => p.Receipt)
                .Where(p => p.Receipt.UserId == user.Id)
                .ToListAsync();

            var productsDto = products.Select(p => MapProductToDto(p)).ToList();
            return productsDto;
        }


        public async Task<List<ProductDto>> GetProductsFromReceipt(int receiptId)
        {
            _validator.IsNull(receiptId, "Brak identyfikatora rachunku");

            var user = await _managementService.GetUser();
            var products = await _dbContext.Products
                .AsNoTracking()
                .Include(p => p.Receipt)
                .Where(p => p.Receipt.UserId == user.Id && p.ReceiptId == receiptId)
                .ToListAsync();

            var productsDto = products.Select(p => MapProductToDto(p)).ToList();
            return productsDto;
        }

        public ProductDto MapProductToDto(Product product)
        {
            var productDto = _mapper.Map<ProductDto>(product);

            var personProducts = _dbContext.PersonProducts
                .Where(pp => pp.ProductId == product.Id) 
                .Include(pp => pp.Person) 
                .ToList();

            productDto.Persons = personProducts.Select(pp => _mapper.Map<PersonDto>(pp.Person)).ToList();
            return productDto;
        }
    }
}
