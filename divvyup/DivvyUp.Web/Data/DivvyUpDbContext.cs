﻿using DivvyUp_Shared.Interfaces;
using DivvyUp_Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace DivvyUp.Web.Data
{
    public class DivvyUpDBContext : DbContext, IDivvyUpDBContext
    {
        public DivvyUpDBContext(DbContextOptions<DivvyUpDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("divvyup");
            modelBuilder.Entity<User>().ToTable("user");
            modelBuilder.Entity<Person>().ToTable("person");
            modelBuilder.Entity<Receipt>().ToTable("receipt");
            modelBuilder.Entity<Loan>().ToTable("loan");
            modelBuilder.Entity<Product>().ToTable("product");
            modelBuilder.Entity<PersonProduct>().ToTable("person_product");
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PersonProduct> PersonProducts { get; set; }
    }
}
