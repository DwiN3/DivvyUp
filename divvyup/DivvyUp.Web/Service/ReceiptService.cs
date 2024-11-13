using System.Globalization;
using AutoMapper;
using DivvyUp.Web.Data;
using DivvyUp.Web.Update;
using DivvyUp.Web.Validation;
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
                Date = request.Date = DateTime.SpecifyKind(request.Date, DateTimeKind.Utc),
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
            receipt.Date = DateTime.SpecifyKind(request.Date, DateTimeKind.Utc);
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

        public async Task<List<ReceiptDto>> GetReceiptsByDataRange(string from, string to)
        {
            _validator.IsEmpty(from, "Brak daty od");
            _validator.IsEmpty(to, "Brak daty do");

            if (!DateTime.TryParseExact(from, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fromDate))
                throw new ArgumentException("Nieprawidłowy format daty początkowej.");

            if (!DateTime.TryParseExact(to, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime toDate))
                throw new ArgumentException("Nieprawidłowy format daty końcowej.");

            fromDate = DateTime.SpecifyKind(fromDate, DateTimeKind.Utc);
            toDate = DateTime.SpecifyKind(toDate, DateTimeKind.Utc);

            var user = await _userContext.GetCurrentUser();
            var receipts = await _dbContext.Receipts
                .AsNoTracking()
                .Where(p => p.UserId == user.Id && p.Date >= fromDate && p.Date <= toDate)
                .ToListAsync();

            var receiptsDto = _mapper.Map<List<ReceiptDto>>(receipts).ToList();
            return receiptsDto;
        }
    }
}
