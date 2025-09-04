using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steria.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModelFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_AspNetUsers_AssingId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Models_MakeId",
                table: "Models");

            migrationBuilder.DropIndex(
                name: "IX_Models_Name",
                table: "Models");

            migrationBuilder.RenameColumn(
                name: "AssingId",
                table: "Cars",
                newName: "ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_AssingId",
                table: "Cars",
                newName: "IX_Cars_ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_MakeId_Name",
                table: "Models",
                columns: new[] { "MakeId", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_AspNetUsers_ManagerId",
                table: "Cars",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_AspNetUsers_ManagerId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Models_MakeId_Name",
                table: "Models");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "Cars",
                newName: "AssingId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_ManagerId",
                table: "Cars",
                newName: "IX_Cars_AssingId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_MakeId",
                table: "Models",
                column: "MakeId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_Name",
                table: "Models",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_AspNetUsers_AssingId",
                table: "Cars",
                column: "AssingId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
