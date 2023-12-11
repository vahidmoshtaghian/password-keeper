using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqlServerOrm.Migrations
{
    /// <inheritdoc />
    public partial class AddFriendOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Friends_UserId",
                schema: "Actor",
                table: "Friends");

            migrationBuilder.AddColumn<long>(
                name: "FriendId",
                schema: "Actor",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "OwnerId",
                schema: "Actor",
                table: "Friends",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Friends_OwnerId",
                schema: "Actor",
                table: "Friends",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_UserId",
                schema: "Actor",
                table: "Friends",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Users_OwnerId",
                schema: "Actor",
                table: "Friends",
                column: "OwnerId",
                principalSchema: "Actor",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Users_OwnerId",
                schema: "Actor",
                table: "Friends");

            migrationBuilder.DropIndex(
                name: "IX_Friends_OwnerId",
                schema: "Actor",
                table: "Friends");

            migrationBuilder.DropIndex(
                name: "IX_Friends_UserId",
                schema: "Actor",
                table: "Friends");

            migrationBuilder.DropColumn(
                name: "FriendId",
                schema: "Actor",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                schema: "Actor",
                table: "Friends");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_UserId",
                schema: "Actor",
                table: "Friends",
                column: "UserId");
        }
    }
}
