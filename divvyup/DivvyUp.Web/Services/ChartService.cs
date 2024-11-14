using DivvyUp.Web.Data;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Interfaces;
using DivvyUp_Shared.Models;
using Microsoft.EntityFrameworkCore;


namespace DivvyUp.Web.Services
{
    public class ChartService : IChartService
    {
        private readonly DuDbContext _dbContext;
        private readonly UserContext _userContext;

        private static readonly string[] MonthNames = { "Sty", "Lut", "Mar", "Kwi", "Maj", "Cze", "Lip", "Sie", "Wrz", "Paź", "Lis", "Gru" };
        private static readonly string[] WeekNames = { "Pon", "Wt", "Śr", "Czw", "Pt", "Sb", "Ndz" };

        public ChartService(DuDbContext dbContext, UserContext userContext)
        {
            _dbContext = dbContext;
            _userContext = userContext;
        }

        public async Task<List<ChartDto>> GetAmounts(bool isTotalAmounts)
        {
            var user = await _userContext.GetCurrentUser();
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
            var user = await _userContext.GetCurrentUser();
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

            var user = await _userContext.GetCurrentUser();

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
            var user = await _userContext.GetCurrentUser();
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
            var user = await _userContext.GetCurrentUser();
            var receipts = await _dbContext.Receipts
                .AsNoTracking()
                .Where(r => r.UserId == user.Id).ToListAsync();

            var dailyTotals = new Dictionary<DayOfWeek, decimal>();
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                dailyTotals[day] = 0;
            }

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
            var endOfWeek = startOfWeek.AddDays(6);

            foreach (var receipt in receipts)
            {
                var date = receipt.Date;

                if (date >= startOfWeek && date <= endOfWeek)
                {
                    var dayOfWeek = date.DayOfWeek;
                    dailyTotals[dayOfWeek] += receipt.TotalPrice;
                }
            }

            var responseList = new List<ChartDto>();
            foreach (var day in Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>())
            {
                decimal total = dailyTotals[day];
                responseList.Add(new ChartDto
                {
                    Name = WeekNames[(int)day],
                    Value = total
                });
            }

            return responseList;
        }

        public async Task<List<ChartDto>> GetWeeklyUserExpenses()
        {
            var user = await _userContext.GetCurrentUser();
            var persons = await _dbContext.Persons
                .AsNoTracking()
                .Where(p => p.UserId == user.Id)
                .ToListAsync();
            var userAccountPersons = persons.Where(p => p.UserAccount).ToList();

            var dailyTotals = new Dictionary<DayOfWeek, decimal>();
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                dailyTotals[day] = 0;
            }

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
            var endOfWeek = startOfWeek.AddDays(6);

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
                            dailyTotals[dayOfWeek] += personProduct.PartOfPrice;
                        }
                    }
                }
            }

            var responseList = new List<ChartDto>();
            foreach (var day in Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>())
            {
                decimal total = dailyTotals[day];
                responseList.Add(new ChartDto
                {
                    Name = WeekNames[(int)day],
                    Value = total
                });
            }

            return responseList;
        }

        public async Task<List<ChartDto>> GetMonthlyTopProducts()
        {
            var user = await _userContext.GetCurrentUser();
            var persons = await _dbContext.Persons.Where(p => p.UserId == user.Id).ToListAsync();

            var allPersonProducts = new List<PersonProduct>();
            foreach (var person in persons)
            {
                var personProducts = await _dbContext.PersonProducts
                    .AsNoTracking()
                    .Include(pp => pp.Product)
                    .Include(pp => pp.Product.Receipt)
                    .Where(pp => pp.PersonId == person.Id)
                    .ToListAsync();
                allPersonProducts.AddRange(personProducts);
            }

            var now = DateTime.Now;
            var currentMonth = now.Month;
            var currentYear = now.Year;

            var productValues = new Dictionary<string, decimal>();

            foreach (var personProduct in allPersonProducts)
            {
                var productDate = personProduct.Product.Receipt.Date;
                if (productDate.Month == currentMonth && productDate.Year == currentYear)
                {
                    productValues[personProduct.Product.Name] = productValues.GetValueOrDefault(personProduct.Product.Name, 0.0m) + personProduct.PartOfPrice;
                }
            }

            var topProducts = productValues
                .OrderByDescending(entry => entry.Value)
                .Take(3)
                .Select(entry => new ChartDto
                {
                    Name = entry.Key,
                    Value = entry.Value
                })
                .ToList();

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
