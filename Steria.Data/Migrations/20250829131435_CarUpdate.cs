using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steria.Data.Migrations
{
    /// <inheritdoc />
    public partial class CarUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInspected",
                table: "Cars",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OnReserve",
                table: "Cars",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInspected",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "OnReserve",
                table: "Cars");
        }
    }
}
