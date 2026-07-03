using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pymex.Payroll.Migrations
{
    /// <inheritdoc />
    public partial class Seagragalamonedaalasvariables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoinId",
                schema: "enterprise",
                table: "PayrollVariables",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_PayrollVariables_CoinId",
                schema: "enterprise",
                table: "PayrollVariables",
                column: "CoinId");

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollVariables_Coins_CoinId",
                schema: "enterprise",
                table: "PayrollVariables",
                column: "CoinId",
                principalSchema: "enterprise",
                principalTable: "Coins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayrollVariables_Coins_CoinId",
                schema: "enterprise",
                table: "PayrollVariables");

            migrationBuilder.DropIndex(
                name: "IX_PayrollVariables_CoinId",
                schema: "enterprise",
                table: "PayrollVariables");

            migrationBuilder.DropColumn(
                name: "CoinId",
                schema: "enterprise",
                table: "PayrollVariables");
        }
    }
}
