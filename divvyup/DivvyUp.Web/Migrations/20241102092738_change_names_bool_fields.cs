using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DivvyUp.Web.Migrations
{
    /// <inheritdoc />
    public partial class change_names_bool_fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "divisible",
                schema: "divvyup",
                table: "product",
                newName: "is_divisible");

            migrationBuilder.RenameColumn(
                name: "user_account",
                schema: "divvyup",
                table: "person",
                newName: "is_user_account");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_divisible",
                schema: "divvyup",
                table: "product",
                newName: "divisible");

            migrationBuilder.RenameColumn(
                name: "is_user_account",
                schema: "divvyup",
                table: "person",
                newName: "user_account");
        }
    }
}
