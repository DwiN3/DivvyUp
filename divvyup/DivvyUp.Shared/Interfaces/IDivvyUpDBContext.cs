using DivvyUp_Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivvyUp_Shared.Interfaces
{
    public interface IDivvyUpDBContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Receipt> Receipts { get; set; }
        DbSet<Person> Persons { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<PersonProduct> PersonProducts { get; set; }
        DbSet<Loan> Loans { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
