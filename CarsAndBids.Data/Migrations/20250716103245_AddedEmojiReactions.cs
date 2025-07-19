using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CarsAndBids.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedEmojiReactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmojiReaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Emoji = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    MessageReactionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmojiReaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmojiReaction_UserChatMessageReactions_MessageReactionId",
                        column: x => x.MessageReactionId,
                        principalTable: "UserChatMessageReactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmojiReaction_MessageReactionId",
                table: "EmojiReaction",
                column: "MessageReactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmojiReaction");
        }
    }
}
