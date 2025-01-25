using System.Net;
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

namespace DivvyUp.Web.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly IDivvyUpDBContext _dbContext;
        private readonly EntityManagementService _managementService;
        private readonly DValidator _validator;
        private readonly IMapper _mapper;

        public ReceiptService(IDivvyUpDBContext dbContext, EntityManagementService managementService, DValidator validator, IMapper mapper)
        {
            _dbContext = dbContext;
            _managementService = managementService;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task Add(AddEditReceiptDto request)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsNull(request.Date, "Data jet wymagana");
            _validator.IsEmpty(request.Name, "Nazwa jest wymagana");
            var user = await _managementService.GetUser();

            var newReceipt = new Receipt()
            {
                User = user,
                Name = request.Name,
                Date = request.Date,
                TotalPrice = 0,
                DiscountPercentage = request.DiscountPercentage,
                Settled = false,
            };

            _dbContext.Receipts.Add(newReceipt);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Edit(AddEditReceiptDto request, int receiptId)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsNull(request.Date, "Data jest wymagana");
            _validator.IsEmpty(request.Name, "Nazwa jest wymagana");
            _validator.IsNull(receiptId, "Brak identyfikatora rachunku");

            var user = await _managementService.GetUser();
            var receipt = await _managementService.GetReceipt(user, receiptId);

            receipt.Name = request.Name;
            receipt.Date = request.Date;

            var discountPercentageChanged = receipt.DiscountPercentage != request.DiscountPercentage;
            if (discountPercentageChanged)
            {
                var products = await _dbContext.Products
                    .Where(c => c.ReceiptId == receiptId)
                    .ToListAsync();
                foreach (var product in products)
                {
                    var totalPrice = _managementService.CalculateTotalPrice(product.Price, product.PurchasedQuantity, product.AdditionalPrice, request.DiscountPercentage);
                    product.DiscountPercentage = request.DiscountPercentage;
                    product.TotalPrice = totalPrice;
                }

                _dbContext.Products.UpdateRange(products);
                await _dbContext.SaveChangesAsync();

                await _managementService.UpdatePartPricesPersonProduct(products);
                await _managementService.UpdateProductDetails(products);
                await _managementService.UpdateTotalPriceReceipt(receipt);
            }

            receipt.DiscountPercentage = request.DiscountPercentage;

            _dbContext.Receipts.Update(receipt);
            await _dbContext.SaveChangesAsync();

            if (discountPercentageChanged)
            {
                await _managementService.UpdatePerson(user, false);
            }
        }

        public async Task Remove(int receiptId)
        {
            _validator.IsNull(receiptId, "Brak identyfikatora rachunku");
            var user = await _managementService.GetUser();
            var receipt = await _managementService.GetReceipt(user, receiptId);

            var products = await _dbContext.Products
                .Where(p => p.ReceiptId == receiptId)
                .ToListAsync();

            var personProductsToRemove = new List<PersonProduct>();

            foreach (var product in products)
            {
                var personProducts = await _dbContext.PersonProducts
                    .Where(pp => pp.ProductId == product.Id)
                    .ToListAsync();

                personProductsToRemove.AddRange(personProducts);
            }

            _dbContext.PersonProducts.RemoveRange(personProductsToRemove);
            _dbContext.Products.RemoveRange(products);
            _dbContext.Receipts.Remove(receipt);

            await _dbContext.SaveChangesAsync();
            await _managementService.UpdatePerson(user, false);
        }

        public async Task SetSettled(int receiptId, bool settled)
        {
            _validator.IsNull(receiptId, "Brak identyfikatora rachunku");
            var user = await _managementService.GetUser();
            var receipt = await _managementService.GetReceipt(user, receiptId);

            receipt.Settled = settled;

            var products = await _dbContext.Products.Where(p => p.ReceiptId == receiptId).ToListAsync();
            foreach (var product in products)
            {
                product.Settled = settled;

                var personProducts = await _dbContext.PersonProducts
                    .Where(pp => pp.ProductId == product.Id)
                    .ToListAsync();

                foreach (var personProduct in personProducts)
                {
                    personProduct.Settled = settled;
                }

                _dbContext.PersonProducts.UpdateRange(personProducts);
            }

            _dbContext.Receipts.Update(receipt);
            _dbContext.Products.UpdateRange(products);
            await _dbContext.SaveChangesAsync();
            await _managementService.UpdatePerson(user, false);
        }

        public async Task<ReceiptDto> GetReceipt(int receiptId)
        {
            _validator.IsNull(receiptId, "Brak identyfikatora rachunku");
            var user = await _managementService.GetUser();
            var receipt = await _managementService.GetReceipt(user, receiptId);
            var receiptDto = _mapper.Map<ReceiptDto>(receipt);
            return receiptDto;
        }

        public async Task<List<ReceiptDto>> GetReceipts()
        {
            var user = await _managementService.GetUser();
            var receipts = await _dbContext.Receipts
                .AsNoTracking()
                .Where(p => p.UserId == user.Id)
                .ToListAsync();

            var receiptsDto = _mapper.Map<List<ReceiptDto>>(receipts).ToList();
            return receiptsDto;
        }

        public async Task<List<ReceiptDto>> GetReceiptsByDataRange(DateOnly from, DateOnly to)
        {
            _validator.IsNull(from, "Brak daty od");
            _validator.IsNull(to, "Brak daty do");
            var user = await _managementService.GetUser();

            if (from > to)
            {
                throw new DException(HttpStatusCode.BadRequest, "Data od nie może być późniejsza niż data do");
            }

            var receipts = await _dbContext.Receipts
                .AsNoTracking()
                .Where(p => p.UserId == user.Id && p.Date >= from && p.Date <= to)
                .ToListAsync();

            var receiptsDto = _mapper.Map<List<ReceiptDto>>(receipts);
            return receiptsDto;
        }

    }
}
