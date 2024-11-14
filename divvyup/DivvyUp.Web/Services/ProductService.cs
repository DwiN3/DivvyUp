using AutoMapper;
using DivvyUp.Web.Data;
using DivvyUp.Web.Update;
using DivvyUp.Web.Validation;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;
using DivvyUp_Shared.Interfaces;
using DivvyUp_Shared.Models;
using Microsoft.EntityFrameworkCore;


namespace DivvyUp.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly DuDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly DuValidator _validator;
        private readonly EntityUpdateService _entityUpdateService;
        private readonly UserContext _userContext;

        public ProductService(DuDbContext dbContext, IMapper mapper, DuValidator validator, EntityUpdateService entityUpdateService, UserContext userContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _validator = validator;
            _entityUpdateService = entityUpdateService;
            _userContext = userContext;
        }


        public async Task<ProductDto> Add(AddEditProductDto request, int receiptId)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsEmpty(request.Name, "Nazwa produktu jest wymagana");
            _validator.IsNull(request.Price, "Cena jest wymagana");
            _validator.IsNull(request.MaxQuantity, "Maksymalna ilość jest wymagana");
            _validator.IsNull(request.Divisible, "Informacja o podzielności jest wymagana");
            _validator.IsNull(receiptId, "Brak identyfikatora rachunku");
            var user = await _userContext.GetCurrentUser();
            var receipt = await _validator.GetReceipt(user, receiptId);

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
            await _entityUpdateService.UpdateTotalPriceReceipt(receipt);
            var productDto = MapProductToDto(newProduct);
            return productDto;
        }

        public async Task<ProductDto> Edit(AddEditProductDto request, int productId)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsEmpty(request.Name, "Nazwa produktu jest wymagana");
            _validator.IsNull(request.Price, "Cena jest wymagana");
            _validator.IsNull(request.MaxQuantity, "Maksymalna ilość jest wymagana");
            _validator.IsNull(request.Divisible, "Informacja o podzielności jest wymagana");
            _validator.IsNull(productId, "Brak identyfikatora produktu");
            var user = await _userContext.GetCurrentUser();
            var product = await _validator.GetProduct(user, productId);

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
            await _entityUpdateService.UpdatePartPricesPersonProduct(product);
            await _entityUpdateService.UpdateProductDetails(product);
            await _entityUpdateService.UpdateTotalPriceReceipt(product.Receipt);
            await _entityUpdateService.UpdatePerson(user, false);
            var productDto = MapProductToDto(product);
            return productDto;
        }

        public async Task Remove(int productId)
        {
            _validator.IsNull(productId, "Brak identyfikatora produktu");
            var user = await _userContext.GetCurrentUser();
            var product = await _validator.GetProduct(user, productId);
            var receipt = await _validator.GetReceipt(user, product.ReceiptId);

            var personProducts = await _dbContext.PersonProducts.Where(pp => pp.ProductId == productId).ToListAsync();
            _dbContext.PersonProducts.RemoveRange(personProducts);
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            await _entityUpdateService.UpdateTotalPriceReceipt(receipt);
            await _entityUpdateService.UpdatePerson(user, false);
        }

        public async Task SetSettled(int productId, bool settled)
        {
            _validator.IsNull(productId, "Brak identyfikatora produktu");
            _validator.IsNull(settled, "Brak decyzji rozliczenia");
            var user = await _userContext.GetCurrentUser();

            var product = await _validator.GetProduct(user, productId);

            product.Settled = settled;
            _dbContext.Products.Update(product);

            var personProducts = await _dbContext.PersonProducts.Where(pp => pp.ProductId == product.Id).ToListAsync();
            foreach (var personProduct in personProducts)
            {
                personProduct.Settled = settled;
                _dbContext.PersonProducts.Update(personProduct);
            }

            var receipt = product.Receipt;
            bool allSettled = await _entityUpdateService.AreAllProductsSettled(receipt);
            receipt.Settled = allSettled;
            _dbContext.Receipts.Update(receipt);

            await _dbContext.SaveChangesAsync();
            await _entityUpdateService.UpdatePerson(user, false);
        }

        public async Task<ProductDto> GetProduct(int productId)
        {
            _validator.IsNull(productId, "Brak identyfikatora produktu");
            var user = await _userContext.GetCurrentUser();
            var product = await _validator.GetProduct(user, productId);
            var productDto = MapProductToDto(product);
            return productDto;
        }


        public async Task<List<ProductDto>> GetProducts()
        {
            var user = await _userContext.GetCurrentUser();
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

            var user = await _userContext.GetCurrentUser();
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
