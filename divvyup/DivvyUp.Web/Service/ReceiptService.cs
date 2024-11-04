using System.Globalization;
using System.Security.Claims;
using AutoMapper;
using DivvyUp.Web.InterfaceWeb;
using DivvyUp.Web.Update;
using DivvyUp.Web.Validator;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Model;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DivvyUp.Web.Service
{
    public class ReceiptService : IReceiptService
    {
        private readonly MyDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IValidator _validator;
        private readonly IEntityUpdateService _entityUpdateService;

        public ReceiptService(MyDbContext dbContext, IMapper mapper, IValidator validator, IEntityUpdateService entityUpdateService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _validator = validator;
            _entityUpdateService = entityUpdateService;
        }

        public async Task<IActionResult> Add(ClaimsPrincipal claims, AddEditReceiptRequest request)
        {
            try
            {
                _validator.IsNull(request, "Nie przekazano danych");
                _validator.IsNull(request.Date, "Data jet wymagana");
                _validator.IsEmpty(request.Name, "Nazwa jest wymagana");

                var user = await _validator.GetUser(claims);

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
                return new OkObjectResult("Pomyślnie dodano rachunek");
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

        public async Task<IActionResult> Edit(ClaimsPrincipal claims, AddEditReceiptRequest request, int receiptId)
        {
            try
            {
                _validator.IsNull(request, "Nie przekazano danych");
                _validator.IsNull(request.Date, "Data jet wymagana");
                _validator.IsEmpty(request.Name, "Nazwa jest wymagana");
                _validator.IsNull(receiptId, "Brak identyfikatora rachunku");

                var receipt = await _validator.GetReceipt(claims, receiptId);
                if (string.IsNullOrEmpty(request.Name))
                    return new BadRequestObjectResult("Nazwa nie może być pusta.");

                receipt.Name = request.Name;
                receipt.Date = DateTime.SpecifyKind(request.Date, DateTimeKind.Utc);
                _dbContext.Receipts.Update(receipt);

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

        public async Task<IActionResult> Remove(ClaimsPrincipal claims, int receiptId)
        {
            try
            {
                _validator.IsNull(receiptId, "Brak identyfikatora rachunku");

                var receipt = await _validator.GetReceipt(claims, receiptId);

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
                await _entityUpdateService.UpdatePerson(claims, false);
                return new OkObjectResult("Pomyślnie usunięto rachunek");
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

        public async Task<IActionResult> SetSettled(ClaimsPrincipal claims, int receiptId, bool settled)
        {
            try
            {
                _validator.IsNull(receiptId, "Brak identyfikatora rachunku");

                var receipt = await _validator.GetReceipt(claims, receiptId);
                receipt.Settled = settled;

                var products = await _dbContext.Products.Where(p => p.ReceiptId == receiptId).ToListAsync();
                foreach (var product in products)
                {
                    product.Settled = settled;
                }

                _dbContext.Receipts.Update(receipt);
                _dbContext.Products.UpdateRange(products);
                await _dbContext.SaveChangesAsync();
                await _entityUpdateService.UpdatePerson(claims, false);
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

        public async Task<IActionResult> GetReceipt(ClaimsPrincipal claims, int receiptId)
        {
            try
            {
                _validator.IsNull(receiptId, "Brak identyfikatora rachunku");

                var receipt = await _validator.GetReceipt(claims, receiptId);
                var receiptDto = _mapper.Map<ReceiptDto>(receipt);
                return new OkObjectResult(receiptDto);
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

        public async Task<IActionResult> GetReceipts(ClaimsPrincipal claims)
        {
            try
            {
                var user = await _validator.GetUser(claims);
                var receipts = await _dbContext.Receipts
                    .Where(p => p.User == user)
                    .ToListAsync();

                var receiptsDto = _mapper.Map<List<ReceiptDto>>(receipts).ToList();
                return new OkObjectResult(receiptsDto);
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

        public async Task<IActionResult> GetReceiptsByDataRange(ClaimsPrincipal claims, string from, string to)
        {
            try
            {
                _validator.IsEmpty(from, "Brak daty od");
                _validator.IsEmpty(to, "Brak daty do");

                if (!DateTime.TryParseExact(from, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fromDate))
                    throw new ArgumentException("Nieprawidłowy format daty początkowej.");

                if (!DateTime.TryParseExact(to, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime toDate))
                    throw new ArgumentException("Nieprawidłowy format daty końcowej.");

                fromDate = DateTime.SpecifyKind(fromDate, DateTimeKind.Utc);
                toDate = DateTime.SpecifyKind(toDate, DateTimeKind.Utc);

                var user = await _validator.GetUser(claims);
                var receipts = await _dbContext.Receipts
                    .Where(p => p.User == user && p.Date >= fromDate && p.Date <= toDate)
                    .ToListAsync();

                var receiptsDto = _mapper.Map<List<ReceiptDto>>(receipts).ToList();
                return new OkObjectResult(receiptsDto);
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
