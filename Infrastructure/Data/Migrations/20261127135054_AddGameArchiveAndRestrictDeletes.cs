using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGameArchiveAndRestrictDeletes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PsnAccounts_Games_GameId",
                table: "PsnAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_PsnAccounts_PsnAccountId",
                table: "Rentals");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Games",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_PsnAccounts_Games_GameId",
                table: "PsnAccounts",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_PsnAccounts_PsnAccountId",
                table: "Rentals",
                column: "PsnAccountId",
                principalTable: "PsnAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PsnAccounts_Games_GameId",
                table: "PsnAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_PsnAccounts_PsnAccountId",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Games");

            migrationBuilder.AddForeignKey(
                name: "FK_PsnAccounts_Games_GameId",
                table: "PsnAccounts",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_PsnAccounts_PsnAccountId",
                table: "Rentals",
                column: "PsnAccountId",
                principalTable: "PsnAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
