using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steria.Data.Migrations
{
    /// <inheritdoc />
    public partial class CarUpdateV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "Cars",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "About",
                table: "Cars");
        }
    }
}
