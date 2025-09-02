using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steria.Data.Migrations
{
    /// <inheritdoc />
    public partial class SomeUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_BodyStyles_BodyStyleId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Chats_ChatId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "OnReserve",
                table: "Cars",
                newName: "IsOnSaleElsewhere");

            migrationBuilder.RenameColumn(
                name: "IsInspected",
                table: "Cars",
                newName: "IsModified");

            migrationBuilder.AddColumn<int>(
                name: "ReplyId",
                table: "Comments",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Speeds",
                table: "Cars",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Cars",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "InteriorColor",
                table: "Cars",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ExteriorColor",
                table: "Cars",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Engine",
                table: "Cars",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Drivetrain",
                table: "Cars",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<int>(
                name: "ChatId",
                table: "Cars",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "BodyStyleId",
                table: "Cars",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Equipment",
                table: "Cars",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Flaws",
                table: "Cars",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Highlights",
                table: "Cars",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifications",
                table: "Cars",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherItems",
                table: "Cars",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnershipHistory",
                table: "Cars",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SellerNotes",
                table: "Cars",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceHistory",
                table: "Cars",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoLinks",
                table: "Cars",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsInspected",
                table: "Auctions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ReplyId",
                table: "Comments",
                column: "ReplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_BodyStyles_BodyStyleId",
                table: "Cars",
                column: "BodyStyleId",
                principalTable: "BodyStyles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Chats_ChatId",
                table: "Cars",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_ReplyId",
                table: "Comments",
                column: "ReplyId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_BodyStyles_BodyStyleId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Chats_ChatId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_ReplyId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ReplyId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ReplyId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Equipment",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Flaws",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Highlights",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Modifications",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "OtherItems",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "OwnershipHistory",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "SellerNotes",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "ServiceHistory",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "VideoLinks",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "IsInspected",
                table: "Auctions");

            migrationBuilder.RenameColumn(
                name: "IsOnSaleElsewhere",
                table: "Cars",
                newName: "OnReserve");

            migrationBuilder.RenameColumn(
                name: "IsModified",
                table: "Cars",
                newName: "IsInspected");

            migrationBuilder.AlterColumn<int>(
                name: "Speeds",
                table: "Cars",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Cars",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InteriorColor",
                table: "Cars",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExteriorColor",
                table: "Cars",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Engine",
                table: "Cars",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Drivetrain",
                table: "Cars",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ChatId",
                table: "Cars",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BodyStyleId",
                table: "Cars",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Cars",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_BodyStyles_BodyStyleId",
                table: "Cars",
                column: "BodyStyleId",
                principalTable: "BodyStyles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Chats_ChatId",
                table: "Cars",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
