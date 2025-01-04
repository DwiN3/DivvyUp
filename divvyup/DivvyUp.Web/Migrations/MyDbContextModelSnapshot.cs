﻿// <auto-generated />
using System;
using DivvyUp.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DivvyUp.Web.Migrations
{
    [DbContext(typeof(DivvyUpDBContext))]
    partial class MyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("divvyup")
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DivvyUp_Shared.Models.Loan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric")
                        .HasColumnName("amount");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<bool>("Lent")
                        .HasColumnType("boolean")
                        .HasColumnName("is_lent");

                    b.Property<int>("PersonId")
                        .HasColumnType("integer")
                        .HasColumnName("person_id");

                    b.Property<bool>("Settled")
                        .HasColumnType("boolean")
                        .HasColumnName("is_settled");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("loan", "divvyup");
                });

            modelBuilder.Entity("DivvyUp_Shared.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("CompensationAmount")
                        .HasColumnType("numeric")
                        .HasColumnName("compensation_amount");

                    b.Property<decimal>("LoanBalance")
                        .HasColumnType("numeric")
                        .HasColumnName("loan_balance");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("ProductsCount")
                        .HasColumnType("integer")
                        .HasColumnName("products_count");

                    b.Property<int>("ReceiptsCount")
                        .HasColumnType("integer")
                        .HasColumnName("receipts_count");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("surname");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("numeric")
                        .HasColumnName("total_amount");

                    b.Property<decimal>("UnpaidAmount")
                        .HasColumnType("numeric")
                        .HasColumnName("unpaid_amount");

                    b.Property<bool>("UserAccount")
                        .HasColumnType("boolean")
                        .HasColumnName("is_user_account");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("person", "divvyup");
                });

            modelBuilder.Entity("DivvyUp_Shared.Models.PersonProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Compensation")
                        .HasColumnType("boolean")
                        .HasColumnName("is_compensation");

                    b.Property<decimal>("PartOfPrice")
                        .HasColumnType("numeric")
                        .HasColumnName("part_of_price");

                    b.Property<int>("PersonId")
                        .HasColumnType("integer")
                        .HasColumnName("person_id");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer")
                        .HasColumnName("product_id");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.Property<bool>("Settled")
                        .HasColumnType("boolean")
                        .HasColumnName("is_settled");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.HasIndex("ProductId");

                    b.ToTable("person_product", "divvyup");
                });

            modelBuilder.Entity("DivvyUp_Shared.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("AdditionalPrice")
                        .HasColumnType("numeric")
                        .HasColumnName("additional_price");

                    b.Property<int>("AvailableQuantity")
                        .HasColumnType("integer")
                        .HasColumnName("available_quantity");

                    b.Property<decimal>("CompensationPrice")
                        .HasColumnType("numeric")
                        .HasColumnName("compensation_price");

                    b.Property<int>("DiscountPercentage")
                        .HasColumnType("integer")
                        .HasColumnName("discount_percentage");

                    b.Property<bool>("Divisible")
                        .HasColumnType("boolean")
                        .HasColumnName("is_divisible");

                    b.Property<int>("MaxQuantity")
                        .HasColumnType("integer")
                        .HasColumnName("max_quantity");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<int>("PurchasedQuantity")
                        .HasColumnType("integer")
                        .HasColumnName("purchased_quantity");

                    b.Property<int>("ReceiptId")
                        .HasColumnType("integer")
                        .HasColumnName("receipt_id");

                    b.Property<bool>("Settled")
                        .HasColumnType("boolean")
                        .HasColumnName("is_settled");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("numeric")
                        .HasColumnName("total_price");

                    b.HasKey("Id");

                    b.HasIndex("ReceiptId");

                    b.ToTable("product", "divvyup");
                });

            modelBuilder.Entity("DivvyUp_Shared.Models.Receipt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<int>("DiscountPercentage")
                        .HasColumnType("integer")
                        .HasColumnName("discount_percentage");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<bool>("Settled")
                        .HasColumnType("boolean")
                        .HasColumnName("is_settled");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("numeric")
                        .HasColumnName("total_price");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("receipt", "divvyup");
                });

            modelBuilder.Entity("DivvyUp_Shared.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.ToTable("user", "divvyup");
                });

            modelBuilder.Entity("DivvyUp_Shared.Models.Loan", b =>
                {
                    b.HasOne("DivvyUp_Shared.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("DivvyUp_Shared.Models.Person", b =>
                {
                    b.HasOne("DivvyUp_Shared.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DivvyUp_Shared.Models.PersonProduct", b =>
                {
                    b.HasOne("DivvyUp_Shared.Models.Person", "Person")
                        .WithMany("PersonProducts")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DivvyUp_Shared.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("DivvyUp_Shared.Models.Product", b =>
                {
                    b.HasOne("DivvyUp_Shared.Models.Receipt", "Receipt")
                        .WithMany()
                        .HasForeignKey("ReceiptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Receipt");
                });

            modelBuilder.Entity("DivvyUp_Shared.Models.Receipt", b =>
                {
                    b.HasOne("DivvyUp_Shared.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DivvyUp_Shared.Models.Person", b =>
                {
                    b.Navigation("PersonProducts");
                });
#pragma warning restore 612, 618
        }
    }
}
