using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Account_MajorId",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_RoleId",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Auction_ReasId",
                table: "Auction");

            migrationBuilder.DropIndex(
                name: "IX_AuctionsAccounting_AuctionId",
                table: "AuctionsAccounting");

            migrationBuilder.DropIndex(
                name: "IX_AuctionsAccounting_ReasId",
                table: "AuctionsAccounting");

            migrationBuilder.DropIndex(
                name: "IX_DepositAmount_ReasId",
                table: "DepositAmount");

            migrationBuilder.DropIndex(
                name: "IX_DepositAmount_RuleId",
                table: "DepositAmount");

            migrationBuilder.DropIndex(
                name: "IX_MoneyTransaction_TypeId",
                table: "MoneyTransaction");

            migrationBuilder.DropIndex(
                name: "IX_MoneyTransactionDetail_AuctionId",
                table: "MoneyTransactionDetail");

            migrationBuilder.DropIndex(
                name: "IX_MoneyTransactionDetail_ReasId",
                table: "MoneyTransactionDetail");
        }
    }
}
