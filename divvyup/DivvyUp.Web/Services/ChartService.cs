using DivvyUp.Web.Data;
using DivvyUp.Web.EntityManager;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DivvyUp.Web.Services
{
    public class ChartService : IChartService
    {
        private readonly DivvyUpDBContext _dbContext;
        private readonly EntityManagementService _managementService;

        private static readonly string[] MonthNames = { "Sty", "Lut", "Mar", "Kwi", "Maj", "Cze", "Lip", "Sie", "Wrz", "Paź", "Lis", "Gru" };
        private static readonly string[] WeekNames = { "Niedz", "Pon", "Wt", "Śr", "Czw", "Pt", "Sb" };

        public ChartService(DivvyUpDBContext dbContext, EntityManagementService managementService)
        {
            _dbContext = dbContext;
            _managementService = managementService;
        }

        public async Task<List<ChartDto>> GetAmounts(bool isTotalAmounts)
        {
            var user = await _managementService.GetUser();
            var persons = await _dbContext.Persons.Where(p => p.UserId == user.Id).ToListAsync();

            var responseList = new List<ChartDto>();
            foreach (var person in persons)
            {
                decimal amount = isTotalAmounts ? person.TotalAmount : person.UnpaidAmount;

                var response = new ChartDto
                {
                    Name = $"{person.Name} {person.Surname}",
                    Value = amount
                };
                responseList.Add(response);
            }

            return responseList;
        }

        public async Task<List<ChartDto>> GetPercentageExpanses()
        {
            var user = await _managementService.GetUser();
            var persons = await _dbContext.Persons
                .AsNoTracking()
                .Where(p => p.UserId == user.Id).ToListAsync();

            var responseList = new List<ChartDto>();
            foreach (var person in persons)
            {
                decimal totalAmount = person.TotalAmount + (person.LoanBalance < 0 ? person.LoanBalance * -1 : 0);
                decimal unpaidAmount = person.UnpaidAmount + (person.LoanBalance < 0 ? person.LoanBalance * -1 : 0);

                decimal reliabilityPercentage = CalculatePaidPercentage(totalAmount, unpaidAmount);

                var response = new ChartDto
                {
                    Name = $"{person.Name} {person.Surname}",
                    Value = reliabilityPercentage
                };
                responseList.Add(response);
            }
            return responseList;
        }

        public async Task<List<ChartDto>> GetMonthlyTotalExpenses(int year)
        {

            var user = await _managementService.GetUser();

            var receipts = await _dbContext.Receipts
                .AsNoTracking()
                .Where(r => r.UserId == user.Id && r.Date.Year == year)
                .ToListAsync();

            var monthlyTotals = new Dictionary<int, decimal>();

            foreach (var receipt in receipts)
            {
                int month = receipt.Date.Month;
                monthlyTotals[month] = monthlyTotals.GetValueOrDefault(month, 0.0m) + receipt.TotalPrice;
            }

            var responseList = new List<ChartDto>();

            for (int month = 1; month <= 12; month++)
            {
                decimal total = monthlyTotals.GetValueOrDefault(month, 0.0m);
                responseList.Add(new ChartDto
                {
                    Name = MonthNames[month - 1],
                    Value = total
                });
            }
            return responseList;
        }

        public async Task<List<ChartDto>> GetMonthlyUserExpenses(int year)
        {
            var user = await _managementService.GetUser();
            var persons = await _dbContext.Persons
                .AsNoTracking()
                .Where(p => p.UserId == user.Id)
                .ToListAsync();
            var userAccountPersons = persons.Where(p => p.UserAccount).ToList();

            var monthlyTotals = new Dictionary<int, decimal>();

            foreach (var person in userAccountPersons)
            {
                var personProducts = await _dbContext.PersonProducts
                    .AsNoTracking()
                    .Include(pp => pp.Product)
                    .Include(pp => pp.Product.Receipt)
                    .Where(pp => pp.PersonId == person.Id)
                    .ToListAsync();


                foreach (var personProduct in personProducts)
                {
                    if (personProduct?.Product?.Receipt != null)
                    {
                        var date = personProduct.Product.Receipt.Date;
                        if (date.Year == year)
                        {
                            int month = date.Month;
                            monthlyTotals[month] = monthlyTotals.GetValueOrDefault(month, 0.0m) + personProduct.PartOfPrice;
                        }
                    }
                }
            }

            var responseList = new List<ChartDto>();
            for (int month = 1; month <= 12; month++)
            {
                decimal total = monthlyTotals.GetValueOrDefault(month, 0.0m);
                responseList.Add(new ChartDto
                {
                    Name = MonthNames[month - 1],
                    Value = total
                });
            }

            return responseList;
        }

        public async Task<List<ChartDto>> GetWeeklyTotalExpenses()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var startOfWeek = today.AddDays(-((int)today.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7);
            var endOfWeek = startOfWeek.AddDays(6);

            var user = await _managementService.GetUser();

            var receipts = await _dbContext.Receipts
                .AsNoTracking()
                .Where(r => r.UserId == user.Id && r.Date >= startOfWeek && r.Date <= endOfWeek)
                .ToListAsync();

            var dailyTotals = new Dictionary<DayOfWeek, decimal>();

            foreach (var receipt in receipts)
            {
                var dayOfWeek = receipt.Date.DayOfWeek;
                dailyTotals[dayOfWeek] = dailyTotals.GetValueOrDefault(dayOfWeek, 0.0m) + receipt.TotalPrice;
            }

            var responseList = new List<ChartDto>();

            for (int i = 0; i < 7; i++)
            {
                var currentDay = startOfWeek.AddDays(i);
                var total = dailyTotals.GetValueOrDefault(currentDay.DayOfWeek, 0.0m);
                responseList.Add(new ChartDto
                {
                    Name = WeekNames[(int)currentDay.DayOfWeek],
                    Value = total
                });
            }

            return responseList;
        }



        public async Task<List<ChartDto>> GetWeeklyUserExpenses()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var startOfWeek = today.AddDays(-((int)today.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7);
            var endOfWeek = startOfWeek.AddDays(6);

            var user = await _managementService.GetUser();

            var persons = await _dbContext.Persons
                .AsNoTracking()
                .Where(p => p.UserId == user.Id)
                .ToListAsync();
            var userAccountPersons = persons.Where(p => p.UserAccount).ToList();

            var dailyTotals = new Dictionary<DayOfWeek, decimal>();

            foreach (var person in userAccountPersons)
            {
                var personProducts = await _dbContext.PersonProducts
                    .AsNoTracking()
                    .Include(pp => pp.Product)
                    .Include(pp => pp.Product.Receipt)
                    .Where(pp => pp.PersonId == person.Id)
                    .ToListAsync();

                foreach (var personProduct in personProducts)
                {
                    if (personProduct?.Product?.Receipt != null)
                    {
                        var date = personProduct.Product.Receipt.Date;
                        if (date >= startOfWeek && date <= endOfWeek)
                        {
                            var dayOfWeek = date.DayOfWeek;
                            dailyTotals[dayOfWeek] = dailyTotals.GetValueOrDefault(dayOfWeek, 0.0m) + personProduct.PartOfPrice;
                        }
                    }
                }
            }

            var responseList = new List<ChartDto>();

            for (int i = 0; i < 7; i++)
            {
                var currentDay = startOfWeek.AddDays(i);
                var total = dailyTotals.GetValueOrDefault(currentDay.DayOfWeek, 0.0m);
                responseList.Add(new ChartDto
                {
                    Name = WeekNames[(int)currentDay.DayOfWeek],
                    Value = total
                });
            }

            return responseList;
        }

        public async Task<List<ChartDto>> GetMonthlyTopProducts()
        {
            var user = await _managementService.GetUser();

            var now = DateTime.Now;
            var currentMonth = now.Month;
            var currentYear = now.Year;

            var topProducts = await _dbContext.Products
                .Include(p => p.Receipt)
                .Where(p => p.Receipt.UserId == user.Id &&
                            p.Receipt.Date.Month == currentMonth &&
                            p.Receipt.Date.Year == currentYear)
                .Select(p => new { p.Name, p.Price })
                .Distinct()
                .OrderByDescending(p => p.Price)
                .Take(3)
                .Select(p => new ChartDto(p.Name, p.Price))
                .ToListAsync();

            return topProducts;
        }

        private decimal CalculatePaidPercentage(decimal totalAmount, decimal unpaidAmount)
        {
            if (totalAmount == 0) return 100;
            if (unpaidAmount == 0) return 100;

            decimal percentage = (1 - (unpaidAmount / totalAmount)) * 100;
            return Math.Round(percentage, 2);
        }
    }
}
