using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steria.Data.Migrations
{
    /// <inheritdoc />
    public partial class NotificationsUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserNotification_AspNetUsers_UserId",
                table: "UserNotification");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotification_NotificationType_NotificationTypeId",
                table: "UserNotification");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotificationSetting_AspNetUsers_UserId",
                table: "UserNotificationSetting");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotificationSetting_NotificationType_NotificationTypeId",
                table: "UserNotificationSetting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserNotificationSetting",
                table: "UserNotificationSetting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserNotification",
                table: "UserNotification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationType",
                table: "NotificationType");

            migrationBuilder.RenameTable(
                name: "UserNotificationSetting",
                newName: "UserNotificationSettings");

            migrationBuilder.RenameTable(
                name: "UserNotification",
                newName: "UserNotifications");

            migrationBuilder.RenameTable(
                name: "NotificationType",
                newName: "NotificationTypes");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotificationSetting_UserId",
                table: "UserNotificationSettings",
                newName: "IX_UserNotificationSettings_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotificationSetting_NotificationTypeId",
                table: "UserNotificationSettings",
                newName: "IX_UserNotificationSettings_NotificationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotification_UserId",
                table: "UserNotifications",
                newName: "IX_UserNotifications_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotification_NotificationTypeId",
                table: "UserNotifications",
                newName: "IX_UserNotifications_NotificationTypeId");

            migrationBuilder.AddColumn<bool>(
                name: "IsMandatory",
                table: "NotificationTypes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserNotificationSettings",
                table: "UserNotificationSettings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserNotifications",
                table: "UserNotifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationTypes",
                table: "NotificationTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_AspNetUsers_UserId",
                table: "UserNotifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_NotificationTypes_NotificationTypeId",
                table: "UserNotifications",
                column: "NotificationTypeId",
                principalTable: "NotificationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotificationSettings_AspNetUsers_UserId",
                table: "UserNotificationSettings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotificationSettings_NotificationTypes_NotificationType~",
                table: "UserNotificationSettings",
                column: "NotificationTypeId",
                principalTable: "NotificationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_AspNetUsers_UserId",
                table: "UserNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_NotificationTypes_NotificationTypeId",
                table: "UserNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotificationSettings_AspNetUsers_UserId",
                table: "UserNotificationSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotificationSettings_NotificationTypes_NotificationType~",
                table: "UserNotificationSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserNotificationSettings",
                table: "UserNotificationSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserNotifications",
                table: "UserNotifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationTypes",
                table: "NotificationTypes");

            migrationBuilder.DropColumn(
                name: "IsMandatory",
                table: "NotificationTypes");

            migrationBuilder.RenameTable(
                name: "UserNotificationSettings",
                newName: "UserNotificationSetting");

            migrationBuilder.RenameTable(
                name: "UserNotifications",
                newName: "UserNotification");

            migrationBuilder.RenameTable(
                name: "NotificationTypes",
                newName: "NotificationType");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotificationSettings_UserId",
                table: "UserNotificationSetting",
                newName: "IX_UserNotificationSetting_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotificationSettings_NotificationTypeId",
                table: "UserNotificationSetting",
                newName: "IX_UserNotificationSetting_NotificationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotifications_UserId",
                table: "UserNotification",
                newName: "IX_UserNotification_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotifications_NotificationTypeId",
                table: "UserNotification",
                newName: "IX_UserNotification_NotificationTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserNotificationSetting",
                table: "UserNotificationSetting",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserNotification",
                table: "UserNotification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationType",
                table: "NotificationType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotification_AspNetUsers_UserId",
                table: "UserNotification",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotification_NotificationType_NotificationTypeId",
                table: "UserNotification",
                column: "NotificationTypeId",
                principalTable: "NotificationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotificationSetting_AspNetUsers_UserId",
                table: "UserNotificationSetting",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotificationSetting_NotificationType_NotificationTypeId",
                table: "UserNotificationSetting",
                column: "NotificationTypeId",
                principalTable: "NotificationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
