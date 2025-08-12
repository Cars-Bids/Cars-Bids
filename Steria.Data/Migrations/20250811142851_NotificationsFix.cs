using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steria.Data.Migrations
{
    /// <inheritdoc />
    public partial class NotificationsFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomJsonData",
                table: "UserNotification");

            migrationBuilder.DropColumn(
                name: "SourceType",
                table: "UserNotification");

            migrationBuilder.AddColumn<string>(
                name: "CustomDataJson",
                table: "UserNotification",
                type: "jsonb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceType",
                table: "NotificationType",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomDataJson",
                table: "UserNotification");

            migrationBuilder.DropColumn(
                name: "SourceType",
                table: "NotificationType");

            migrationBuilder.AddColumn<string>(
                name: "CustomJsonData",
                table: "UserNotification",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceType",
                table: "UserNotification",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
