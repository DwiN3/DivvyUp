using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DivvyUp.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddCompensationAmountToPerson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "compensation_amount",
                schema: "divvyup",
                table: "person",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "compensation_amount",
                schema: "divvyup",
                table: "person");
        }
    }
}
