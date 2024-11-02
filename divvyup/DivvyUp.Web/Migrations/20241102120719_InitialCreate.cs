using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DivvyUp.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "divvyup");

            migrationBuilder.CreateTable(
                name: "user",
                schema: "divvyup",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "person",
                schema: "divvyup",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    surname = table.Column<string>(type: "text", nullable: false),
                    receipts_count = table.Column<int>(type: "integer", nullable: false),
                    products_count = table.Column<int>(type: "integer", nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    unpaid_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    loan_balance = table.Column<decimal>(type: "numeric", nullable: false),
                    is_user_account = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person", x => x.id);
                    table.ForeignKey(
                        name: "FK_person_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "divvyup",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "receipt",
                schema: "divvyup",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    total_price = table.Column<decimal>(type: "numeric", nullable: false),
                    is_settled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receipt", x => x.id);
                    table.ForeignKey(
                        name: "FK_receipt_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "divvyup",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "loan",
                schema: "divvyup",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    person_id = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    is_lent = table.Column<bool>(type: "boolean", nullable: false),
                    is_settled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_loan", x => x.id);
                    table.ForeignKey(
                        name: "FK_loan_person_person_id",
                        column: x => x.person_id,
                        principalSchema: "divvyup",
                        principalTable: "person",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product",
                schema: "divvyup",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    receipt_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    is_divisible = table.Column<bool>(type: "boolean", nullable: false),
                    max_quantity = table.Column<int>(type: "integer", nullable: false),
                    compensation_price = table.Column<decimal>(type: "numeric", nullable: false),
                    is_settled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_receipt_receipt_id",
                        column: x => x.receipt_id,
                        principalSchema: "divvyup",
                        principalTable: "receipt",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "person_product",
                schema: "divvyup",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    person_id = table.Column<int>(type: "integer", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    part_of_price = table.Column<decimal>(type: "numeric", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    is_compensation = table.Column<bool>(type: "boolean", nullable: false),
                    is_settled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person_product", x => x.id);
                    table.ForeignKey(
                        name: "FK_person_product_person_person_id",
                        column: x => x.person_id,
                        principalSchema: "divvyup",
                        principalTable: "person",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_person_product_product_product_id",
                        column: x => x.product_id,
                        principalSchema: "divvyup",
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_loan_person_id",
                schema: "divvyup",
                table: "loan",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "IX_person_user_id",
                schema: "divvyup",
                table: "person",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_person_product_person_id",
                schema: "divvyup",
                table: "person_product",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "IX_person_product_product_id",
                schema: "divvyup",
                table: "person_product",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_receipt_id",
                schema: "divvyup",
                table: "product",
                column: "receipt_id");

            migrationBuilder.CreateIndex(
                name: "IX_receipt_user_id",
                schema: "divvyup",
                table: "receipt",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "loan",
                schema: "divvyup");

            migrationBuilder.DropTable(
                name: "person_product",
                schema: "divvyup");

            migrationBuilder.DropTable(
                name: "person",
                schema: "divvyup");

            migrationBuilder.DropTable(
                name: "product",
                schema: "divvyup");

            migrationBuilder.DropTable(
                name: "receipt",
                schema: "divvyup");

            migrationBuilder.DropTable(
                name: "user",
                schema: "divvyup");
        }
    }
}
