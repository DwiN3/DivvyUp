using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DivvyUp.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscountAndPurchasedQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "discount_percentage",
                schema: "divvyup",
                table: "product",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "purchased_quantity",
                schema: "divvyup",
                table: "product",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "total_price",
                schema: "divvyup",
                table: "product",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "discount_percentage",
                schema: "divvyup",
                table: "product");

            migrationBuilder.DropColumn(
                name: "purchased_quantity",
                schema: "divvyup",
                table: "product");

            migrationBuilder.DropColumn(
                name: "total_price",
                schema: "divvyup",
                table: "product");
        }
    }
}
