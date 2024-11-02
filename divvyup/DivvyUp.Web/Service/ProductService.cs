using System;
using System.Security.Claims;
using AutoMapper;
using DivvyUp.Web.InterfaceWeb;
using DivvyUp.Web.Validator;
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
        private readonly IValidator _validator;

        public ProductService(MyDbContext dbContext, IMapper mapper, IValidator validator)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _validator = validator;
        }


        public async Task<IActionResult> Add(ClaimsPrincipal claims, AddEditProductRequest request, int receiptId)
        {
            try
            {
                var receipt = await _validator.GetReceipt(claims, receiptId);

                var newProduct = new Product()
                {
                    Receipt = receipt,
                    ReceiptId = receipt.Id,
                    Name = request.Name,
                    Price = request.Price,
                    Divisible = request.Divisible,
                    MaxQuantity = request.MaxQuantity,
                    CompensationPrice = 0,
                    Settled = false,
                };

                _dbContext.Products.Add(newProduct);
                await _dbContext.SaveChangesAsync();
                return new OkObjectResult("Pomyślnie dodano produkt");
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

        public async Task<IActionResult> Edit(ClaimsPrincipal claims, AddEditProductRequest request, int productId)
        {
            try
            {
                var product = await _validator.GetProduct(claims, productId);

                product.Name = request.Name;
                product.Price = request.Price;
                product.Divisible = request.Divisible;
                product.MaxQuantity = request.MaxQuantity;

                _dbContext.Products.Update(product);
                await _dbContext.SaveChangesAsync();
                return new OkObjectResult("Pomyślnie wprowadzono zmiany");
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

        public async Task<IActionResult> Remove(ClaimsPrincipal claims, int productId)
        {
            try
            {
                var product = await _validator.GetProduct(claims, productId);

                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
                return new OkObjectResult("Pomyślnie usunięto produkt");
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

        public async Task<IActionResult> SetSettled(ClaimsPrincipal claims, int productId, bool settled)
        {
            try
            {
                var product = await _validator.GetProduct(claims, productId);
                product.Settled = settled;
                _dbContext.Products.Update(product);
                await _dbContext.SaveChangesAsync();
                return new OkObjectResult("Pomyślnie wprowadzono zmiany");
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

        public async Task<IActionResult> GetProduct(ClaimsPrincipal claims, int productId)
        {
            try
            {
                var product = await _validator.GetProduct(claims, productId);
                var productDto = _mapper.Map<ProductDto>(product);
                return new OkObjectResult(productDto);
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

                var productsDto = _mapper.Map<List<ProductDto>>(products).ToList();
                return new OkObjectResult(productsDto);
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

        public async Task<IActionResult> GetProductsFromReceipt(ClaimsPrincipal claims, int receiptId)
        {
            try
            {
                var user = await _validator.GetUser(claims);
                var products = await _dbContext.Products
                    .Include(p => p.Receipt)
                    .Include(p => p.Receipt.User)
                    .Where(p => p.Receipt.User == user && p.ReceiptId == receiptId)
                    .ToListAsync();

                var productsDto = _mapper.Map<List<ProductDto>>(products).ToList();
                return new OkObjectResult(productsDto);
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
    }
}
