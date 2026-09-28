using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pymex.Payroll.Migrations
{
    /// <inheritdoc />
    public partial class AddRelatedContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RelatedContracts",
                schema: "enterprise",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContractId = table.Column<int>(type: "integer", nullable: false),
                    RelatedContractId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedContracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelatedContracts_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalSchema: "enterprise",
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RelatedContracts_Contracts_RelatedContractId",
                        column: x => x.RelatedContractId,
                        principalSchema: "enterprise",
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RelatedContracts_RelatedContractId",
                schema: "enterprise",
                table: "RelatedContracts",
                column: "RelatedContractId");

            migrationBuilder.CreateIndex(
                name: "UIDX_RelatedContracts",
                schema: "enterprise",
                table: "RelatedContracts",
                columns: new[] { "ContractId", "RelatedContractId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RelatedContracts",
                schema: "enterprise");
        }
    }
}
