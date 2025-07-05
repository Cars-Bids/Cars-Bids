using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarsAndBids.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_CurrentBidder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentBidder",
                table: "Auctions",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentBidder",
                table: "Auctions");
        }
    }
}
