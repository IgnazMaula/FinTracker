using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addBalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "BankTransactions",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "BankTransactions");
        }
    }
}
