using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DivvyUp.Web.Migrations
{
    /// <inheritdoc />
    public partial class ExpandUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                schema: "divvyup",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "surname",
                schema: "divvyup",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                schema: "divvyup",
                table: "user");

            migrationBuilder.DropColumn(
                name: "surname",
                schema: "divvyup",
                table: "user");
        }
    }
}
