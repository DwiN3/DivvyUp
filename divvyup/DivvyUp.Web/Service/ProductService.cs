using System.Security.Claims;
using AutoMapper;
using DivvyUp.Web.Data;
using DivvyUp.Web.Interface;
using DivvyUp.Web.Update;
using DivvyUp.Web.Validation;
using DivvyUp_Impl_Maui.Api.Exceptions;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Model;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DivvyUp.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly MyDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly MyValidator _validator;
        private readonly EntityUpdateService _entityUpdateService;

        public ProductService(MyDbContext dbContext, IMapper mapper, MyValidator validator, EntityUpdateService entityUpdateService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _validator = validator;
            _entityUpdateService = entityUpdateService;
        }


        public async Task<IActionResult> Add(ClaimsPrincipal claims, AddEditProductRequest request, int receiptId)
        {
            try
            {
                _validator.IsNull(request, "Nie przekazano danych");
                _validator.IsEmpty(request.Name, "Nazwa produktu jest wymagana");
                _validator.IsNull(request.Price, "Cena jest wymagana");
                _validator.IsNull(request.MaxQuantity, "Maksymalna ilość jest wymagana");
                _validator.IsNull(request.Divisible, "Informacja o podzielności jest wymagana");
                _validator.IsNull(receiptId, "Brak identyfikatora rachunku");

                var receipt = await _validator.GetReceipt(claims, receiptId);

                var newProduct = new Product()
                {
                    Receipt = receipt,
                    Name = request.Name,
                    Price = request.Price,
                    Divisible = request.Divisible,
                    MaxQuantity = request.Divisible ? request.MaxQuantity : 1,
                    CompensationPrice = request.Divisible ? request.Price : 0,
                    Settled = false,
                };

                _dbContext.Products.Add(newProduct);
                await _dbContext.SaveChangesAsync();
                await _entityUpdateService.UpdateTotalPriceReceipt(receipt);
                return new OkObjectResult(newProduct);
            }
            catch (DException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> Edit(ClaimsPrincipal claims, AddEditProductRequest request, int productId)
        {
            try
            {
                _validator.IsNull(request, "Nie przekazano danych");
                _validator.IsEmpty(request.Name, "Nazwa produktu jest wymagana");
                _validator.IsNull(request.Price, "Cena jest wymagana");
                _validator.IsNull(request.MaxQuantity, "Maksymalna ilość jest wymagana");
                _validator.IsNull(request.Divisible, "Informacja o podzielności jest wymagana");
                _validator.IsNull(productId, "Brak identyfikatora produktu");

                var product = await _validator.GetProduct(claims, productId);
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
                await _entityUpdateService.UpdateCompensationPrice(product);
                await _entityUpdateService.UpdateTotalPriceReceipt(product.Receipt);
                await _entityUpdateService.UpdatePerson(claims, false);
                return new OkObjectResult("Pomyślnie wprowadzono zmiany");
            }
            catch (DException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> Remove(ClaimsPrincipal claims, int productId)
        {
            try
            {
                _validator.IsNull(productId, "Brak identyfikatora produktu");
                var product = await _validator.GetProduct(claims, productId);
                var receipt = await _validator.GetReceipt(claims, product.ReceiptId);

                var personProducts = await _dbContext.PersonProducts.Where(pp => pp.ProductId == productId).ToListAsync();
                _dbContext.PersonProducts.RemoveRange(personProducts);
                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();

                await _entityUpdateService.UpdateTotalPriceReceipt(receipt);
                await _entityUpdateService.UpdatePerson(claims, false);
                return new OkObjectResult("Pomyślnie usunięto produkt");
            }
            catch (DException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> SetSettled(ClaimsPrincipal claims, int productId, bool settled)
        {
            try
            {
                _validator.IsNull(productId, "Brak identyfikatora produktu");
                _validator.IsNull(settled, "Brak decyzji rozliczenia");

                var product = await _validator.GetProduct(claims, productId);

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
                await _entityUpdateService.UpdatePerson(claims, false);
                return new OkObjectResult("Pomyślnie wprowadzono zmiany");
            }
            catch (DException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> GetProduct(ClaimsPrincipal claims, int productId)
        {
            try
            {
                _validator.IsNull(productId, "Brak identyfikatora produktu");
                var product = await _validator.GetProduct(claims, productId);
                var productDto = MapProductToDto(product);

                return new OkObjectResult(productDto);
            }
            catch (DException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }


        public async Task<IActionResult> GetProducts(ClaimsPrincipal claims)
        {
            try
            {
                var user = await _validator.GetUser(claims);
                var products = await _dbContext.Products
                    .Include(p => p.Receipt)
                    .Include(p => p.Receipt.User)
                    .Where(p => p.Receipt.User == user)
                    .ToListAsync();

                var productsDto = products.Select(p => MapProductToDto(p)).ToList();

                return new OkObjectResult(productsDto);
            }
            catch (DException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }


        public async Task<IActionResult> GetProductsFromReceipt(ClaimsPrincipal claims, int receiptId)
        {
            try
            {
                _validator.IsNull(receiptId, "Brak identyfikatora rachunku");

                var user = await _validator.GetUser(claims);
                var products = await _dbContext.Products
                    .Include(p => p.Receipt)
                    .Include(p => p.Receipt.User)
                    .Where(p => p.Receipt.User == user && p.ReceiptId == receiptId)
                    .ToListAsync();

                var productsDto = products.Select(p => MapProductToDto(p)).ToList();
                return new OkObjectResult(productsDto);
            }
            catch (DException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public ProductDto MapProductToDto(Product product)
        {
            var productDto = _mapper.Map<ProductDto>(product);

            var personProducts = _dbContext.PersonProducts
                .Where(pp => pp.ProductId == product.Id) 
                .Include(pp => pp.Person) 
                .ToList();

            productDto.persons = personProducts.Select(pp => _mapper.Map<PersonDto>(pp.Person)).ToList();
            return productDto;
        }
    }
}
