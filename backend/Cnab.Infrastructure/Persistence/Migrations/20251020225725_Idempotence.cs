using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cnab.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Idempotence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccountTransactions_TransactionTypeId",
                table: "AccountTransactions");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTransactions_TransactionTypeId_StoreId_Date_Value_Cpf_Card",
                table: "AccountTransactions",
                columns: new[] { "TransactionTypeId", "StoreId", "Date", "Value", "Cpf", "Card" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccountTransactions_TransactionTypeId_StoreId_Date_Value_Cpf_Card",
                table: "AccountTransactions");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTransactions_TransactionTypeId",
                table: "AccountTransactions",
                column: "TransactionTypeId");
        }
    }
}
