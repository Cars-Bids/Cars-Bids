using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Steria.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSavedSearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SavedSearch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ModelId = table.Column<int>(type: "integer", nullable: true),
                    MakeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedSearch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedSearch_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedSearch_Makes_MakeId",
                        column: x => x.MakeId,
                        principalTable: "Makes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedSearch_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SavedSearch_MakeId",
                table: "SavedSearch",
                column: "MakeId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedSearch_ModelId",
                table: "SavedSearch",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedSearch_UserId",
                table: "SavedSearch",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavedSearch");
        }
    }
}
