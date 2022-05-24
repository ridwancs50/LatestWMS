using Microsoft.EntityFrameworkCore.Migrations;

namespace LatestWMS.Migrations
{
    public partial class transbeneficiary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeneficiaryAccount",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "BeneficiaryAccountAccountNumber",
                table: "Transactions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BeneficiaryAccountAccountNumber",
                table: "Transactions",
                column: "BeneficiaryAccountAccountNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_BeneficiaryAccountAccountNumber",
                table: "Transactions",
                column: "BeneficiaryAccountAccountNumber",
                principalTable: "Accounts",
                principalColumn: "AccountNumber",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_BeneficiaryAccountAccountNumber",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_BeneficiaryAccountAccountNumber",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "BeneficiaryAccountAccountNumber",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "BeneficiaryAccount",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
