using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHealth.ManageItemLists.DataAccess.Migrations
{
    public partial class CreateInvestmentCostPackageSummaryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvestmentCostPackageSummaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InvestmentCostPackageComponentId = table.Column<Guid>(type: "uuid", nullable: false),
                    NumberOfItems = table.Column<int>(type: "integer", nullable: true),
                    TotalNumberOfQuantity = table.Column<int>(type: "integer", nullable: true),
                    TotalCostOfItems = table.Column<double>(type: "double precision", nullable: true),
                    YearlyDepreciationCostForTheAddedAsset = table.Column<double>(type: "double precision", nullable: true),
                    YearlyMaintenanceCostForTheAddedAsset = table.Column<double>(type: "double precision", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsDeletedBy = table.Column<string>(type: "text", nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentCostPackageSummaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentCostPackageSummaries_InvestmentCostPackageCompone~",
                        column: x => x.InvestmentCostPackageComponentId,
                        principalTable: "InvestmentCostPackageComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentCostPackageSummaries_InvestmentCostPackageCompone~",
                table: "InvestmentCostPackageSummaries",
                column: "InvestmentCostPackageComponentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvestmentCostPackageSummaries");
        }
    }
}
