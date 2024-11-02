using DivvyUp_Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace DivvyUp.Web
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("divvyup");
            modelBuilder.Entity<User>().ToTable("user");
            modelBuilder.Entity<Person>().ToTable("person");
            modelBuilder.Entity<Receipt>().ToTable("receipt");
            modelBuilder.Entity<Loan>().ToTable("loan");

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Loan> Loans { get; set; }
    }
}
