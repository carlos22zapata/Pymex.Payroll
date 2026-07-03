using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pymex.Payroll.Migrations
{
    /// <inheritdoc />
    public partial class AddCoinsAndCoinQuotations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coins",
                schema: "enterprise",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Symbol = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    IdWeb = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoinQuotations",
                schema: "enterprise",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Observation = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Value = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    CoinId = table.Column<int>(type: "integer", nullable: false),
                    Origin = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinQuotations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoinQuotations_Coins",
                        column: x => x.CoinId,
                        principalSchema: "enterprise",
                        principalTable: "Coins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_CoinQuotations_CoinId_Date",
                schema: "enterprise",
                table: "CoinQuotations",
                columns: new[] { "CoinId", "Date" });

            migrationBuilder.CreateIndex(
                name: "UIDX_Coins_Name",
                schema: "enterprise",
                table: "Coins",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UIDX_Coins_Symbol",
                schema: "enterprise",
                table: "Coins",
                column: "Symbol",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoinQuotations",
                schema: "enterprise");

            migrationBuilder.DropTable(
                name: "Coins",
                schema: "enterprise");
        }
    }
}
