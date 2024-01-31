using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseAccountTableUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Account_MajorId",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_RoleId",
                table: "Account");
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
        }
    }
}
