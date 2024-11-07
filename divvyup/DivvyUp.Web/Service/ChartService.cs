using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using AutoMapper;
using DivvyUp.Web.Data;
using DivvyUp.Web.Interfac;
using DivvyUp.Web.Validation;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DivvyUp.Web.Service
{
    public class ChartService : IChartService
    {
        private readonly MyDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly MyValidator _validator;

        private static readonly string[] MonthNames = { "Sty", "Lut", "Mar", "Kwi", "Maj", "Cze", "Lip", "Sie", "Wrz", "Paź", "Lis", "Gru" };
        private static readonly string[] WeekNames = { "Pon", "Wt", "Śr", "Czw", "Pt", "Sb", "Ndz" };

        public ChartService(MyDbContext dbContext, IMapper mapper, MyValidator validator)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IActionResult> GetAmounts(ClaimsPrincipal claims, bool isTotalAmounts)
        {
            try
            {
                var user = await _validator.GetUser(claims);
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
                return new OkObjectResult(responseList);
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

        public async Task<IActionResult> GetPercentageExpanses(ClaimsPrincipal claims)
        {
            try
            {
                var user = await _validator.GetUser(claims);
                var persons = await _dbContext.Persons.Where(p => p.UserId == user.Id).ToListAsync();

                var responseList = new List<ChartDto>();
                foreach (var person in persons)
                {
                    decimal amount = CalculatePaidPercentage(person.TotalAmount, person.UnpaidAmount);

                    var response = new ChartDto
                    {
                        Name = $"{person.Name} {person.Surname}",
                        Value = amount
                    };
                    responseList.Add(response);
                }
                return new OkObjectResult(responseList);
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

        public async Task<IActionResult> GetMonthlyTotalExpenses(ClaimsPrincipal claims, int year)
        {
            try
            {
                var user = await _validator.GetUser(claims);

                var receipts = await _dbContext.Receipts
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
                return new OkObjectResult(responseList);
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

        public async Task<IActionResult> GetMonthlyUserExpenses(ClaimsPrincipal claims, int year)
        {
            try
            {
                var user = await _validator.GetUser(claims);
                var persons = await _dbContext.Persons.Where(p => p.UserId == user.Id).ToListAsync();
                var userAccountPersons = persons.Where(p => p.UserAccount).ToList();

                var monthlyTotals = new Dictionary<int, decimal>();

                foreach (var person in userAccountPersons)
                {
                    var personProducts = await _dbContext.PersonProducts
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

                return new OkObjectResult(responseList);
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

        public async Task<IActionResult> GetWeeklyTotalExpenses(ClaimsPrincipal claims)
        {
            try
            {
                var user = await _validator.GetUser(claims);
                var receipts = await _dbContext.Receipts.Where(r => r.UserId == user.Id).ToListAsync();

                var dailyTotals = new Dictionary<DayOfWeek, decimal>();
                foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
                {
                    dailyTotals[day] = 0;
                }

                var today = DateTime.UtcNow;
                var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
                var endOfWeek = startOfWeek.AddDays(6);

                foreach (var receipt in receipts)
                {
                    var date = receipt.Date.Date;

                    if (date >= startOfWeek.Date && date <= endOfWeek.Date)
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

                return new OkObjectResult(responseList);
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


        public async Task<IActionResult> GetWeeklyUserExpenses(ClaimsPrincipal claims)
        {
            try
            {
                var user = await _validator.GetUser(claims);
                var persons = await _dbContext.Persons.Where(p => p.UserId == user.Id).ToListAsync();
                var userAccountPersons = persons.Where(p => p.UserAccount).ToList();

                var dailyTotals = new Dictionary<DayOfWeek, decimal>();
                foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
                {
                    dailyTotals[day] = 0;
                }

                var today = DateTime.UtcNow;
                var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
                var endOfWeek = startOfWeek.AddDays(6);

                foreach (var person in userAccountPersons)
                {
                    var personProducts = await _dbContext.PersonProducts
                        .Include(pp => pp.Product)
                        .Include(pp => pp.Product.Receipt)
                        .Where(pp => pp.PersonId == person.Id)
                        .ToListAsync();

                    foreach (var personProduct in personProducts)
                    {
                        if (personProduct?.Product?.Receipt != null)
                        {
                            var date = personProduct.Product.Receipt.Date.Date;

                            if (date >= startOfWeek.Date && date <= endOfWeek.Date)
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

                return new OkObjectResult(responseList);
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


        public async Task<IActionResult> GetMonthlyTopProducts(ClaimsPrincipal claims)
        {
            try
            {
                var user = await _validator.GetUser(claims);
                var persons = await _dbContext.Persons.Where(p => p.UserId == user.Id).ToListAsync();

                var allPersonProducts = new List<PersonProduct>();
                foreach (var person in persons)
                {
                    var personProducts = await _dbContext.PersonProducts
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

                return new OkObjectResult(topProducts);
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

        private decimal CalculatePaidPercentage(decimal totalAmount, decimal unpaidAmount)
        {
            if (totalAmount == 0) return 100;
            if (unpaidAmount == 0) return 100;

            decimal percentage = (1 - (unpaidAmount / totalAmount)) * 100;
            return Math.Round(percentage, 2);
        }
    }
}
