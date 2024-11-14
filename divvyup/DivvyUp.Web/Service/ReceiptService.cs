using System.Globalization;
using System.Net;
using AutoMapper;
using DivvyUp.Web.Data;
using DivvyUp.Web.Update;
using DivvyUp.Web.Validation;
using DivvyUp_Impl_Maui.Api.Exceptions;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using DivvyUp_Shared.Model;
using DivvyUp_Shared.RequestDto;
using Microsoft.EntityFrameworkCore;


namespace DivvyUp.Web.Service
{
    public class ReceiptService : IReceiptService
    {
        private readonly MyDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly MyValidator _validator;
        private readonly EntityUpdateService _entityUpdateService;
        private readonly UserContext _userContext;

        public ReceiptService(
            UserContext userContext,
            MyDbContext dbContext,
            IMapper mapper,
            MyValidator validator,
            EntityUpdateService entityUpdateService)
        {
            _userContext = userContext;
            _dbContext = dbContext;
            _mapper = mapper;
            _validator = validator;
            _entityUpdateService = entityUpdateService;
        }

        public async Task Add(AddEditReceiptRequest request)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsNull(request.Date, "Data jet wymagana");
            _validator.IsEmpty(request.Name, "Nazwa jest wymagana");
            var user = await _userContext.GetCurrentUser();

            var newReceipt = new Receipt()
            {
                User = user,
                Name = request.Name,
                Date = request.Date,
                TotalPrice = 0,
                Settled = false,
            };

            _dbContext.Receipts.Add(newReceipt);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Edit(AddEditReceiptRequest request, int receiptId)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsNull(request.Date, "Data jet wymagana");
            _validator.IsEmpty(request.Name, "Nazwa jest wymagana");
            _validator.IsNull(receiptId, "Brak identyfikatora rachunku");
            var user = await _userContext.GetCurrentUser();
            var receipt = await _validator.GetReceipt(user, receiptId);

            receipt.Name = request.Name;
            receipt.Date = request.Date;

            _dbContext.Receipts.Update(receipt);

            await _dbContext.SaveChangesAsync();
        }

        public async Task Remove(int receiptId)
        {
            _validator.IsNull(receiptId, "Brak identyfikatora rachunku");
            var user = await _userContext.GetCurrentUser();
            var receipt = await _validator.GetReceipt(user, receiptId);

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
            await _entityUpdateService.UpdatePerson(user, false);
        }

        public async Task SetSettled(int receiptId, bool settled)
        {
            _validator.IsNull(receiptId, "Brak identyfikatora rachunku");
            var user = await _userContext.GetCurrentUser();
            var receipt = await _validator.GetReceipt(user, receiptId);

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
            await _entityUpdateService.UpdatePerson(user, false);
        }

        public async Task<ReceiptDto> GetReceipt(int receiptId)
        {
            _validator.IsNull(receiptId, "Brak identyfikatora rachunku");
            var user = await _userContext.GetCurrentUser();
            var receipt = await _validator.GetReceipt(user, receiptId);
            var receiptDto = _mapper.Map<ReceiptDto>(receipt);
            return receiptDto;
        }

        public async Task<List<ReceiptDto>> GetReceipts()
        {
            var user = await _userContext.GetCurrentUser();
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
            var user = await _userContext.GetCurrentUser();

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
