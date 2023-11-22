using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHealth.ManageItemLists.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addInvestmentCostPackage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvestmentCostPackageComponents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PackageHeaderId = table.Column<Guid>(type: "uuid", nullable: false),
                    FacilityUHIAId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuantityOfUnitsPerTheFacility = table.Column<int>(type: "integer", nullable: false),
                    NumberOfSessionsPerUnitPerFacility = table.Column<int>(type: "integer", nullable: false),
                    InvestmentCostDepreciationAndMaintenanceId = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_InvestmentCostPackageComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentCostPackageComponents_FacilityUHIA_FacilityUHIAId",
                        column: x => x.FacilityUHIAId,
                        principalTable: "FacilityUHIA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentCostPackageComponents_PackageHeaders_PackageHeade~",
                        column: x => x.PackageHeaderId,
                        principalTable: "PackageHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentCostDepreciationsAndMaintenances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InvestmentCostPackageComponentId = table.Column<Guid>(type: "uuid", nullable: false),
                    YearlyDepreciationCostForTotalAddedAssets = table.Column<double>(type: "double precision", nullable: true),
                    MonthlyDepreciationCostForTotalAddedAssets = table.Column<double>(type: "double precision", nullable: true),
                    DailyDepreciationCostForTotalAddedAssets = table.Column<double>(type: "double precision", nullable: true),
                    HourlyDepreciationCostForTotalAddedAssets = table.Column<double>(type: "double precision", nullable: true),
                    MinuteDepreciationCostForTotalAddedAssets = table.Column<double>(type: "double precision", nullable: true),
                    YearlyMaintenanceCostForTotalAddedAssets = table.Column<double>(type: "double precision", nullable: true),
                    MonthlyMaintenanceCostForTotalAddedAssets = table.Column<double>(type: "double precision", nullable: true),
                    DailyMaintenanceCostForTotalAddedAssets = table.Column<double>(type: "double precision", nullable: true),
                    HourlyMaintenanceCostForTotalAddedAssets = table.Column<double>(type: "double precision", nullable: true),
                    MinuteMaintenanceCostForTotalAddedAssets = table.Column<double>(type: "double precision", nullable: true),
                    YearlyDepreciationCostForTotalAddedAssetsPerUnit = table.Column<double>(type: "double precision", nullable: true),
                    MonthlyDepreciationCostForTotalAddedAssetsPerUnit = table.Column<double>(type: "double precision", nullable: true),
                    DailyDepreciationCostForTotalAddedAssetsPerUnit = table.Column<double>(type: "double precision", nullable: true),
                    HourlyDepreciationCostForTotalAddedAssetsPerUnit = table.Column<double>(type: "double precision", nullable: true),
                    MinuteDepreciationCostForTotalAddedAssetsPerUnit = table.Column<double>(type: "double precision", nullable: true),
                    YearlyMaintenanceCostForTotalAddedAssetsPerUnit = table.Column<double>(type: "double precision", nullable: true),
                    MonthlyMaintenanceCostForTotalAddedAssetsPerUnit = table.Column<double>(type: "double precision", nullable: true),
                    DailyMaintenanceCostForTotalAddedAssetsPerUnit = table.Column<double>(type: "double precision", nullable: true),
                    HourlyMaintenanceCostForTotalAddedAssetsPerUnit = table.Column<double>(type: "double precision", nullable: true),
                    MinuteMaintenanceCostForTotalAddedAssetsPerUnit = table.Column<double>(type: "double precision", nullable: true),
                    DailyDepreciationCostForTotalAddedAssetsPerSession = table.Column<double>(type: "double precision", nullable: true),
                    DailyMaintenanceCostForTotalAddedAssetsPerSession = table.Column<double>(type: "double precision", nullable: true),
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
                    table.PrimaryKey("PK_InvestmentCostDepreciationsAndMaintenances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentCostDepreciationsAndMaintenances_InvestmentCostPa~",
                        column: x => x.InvestmentCostPackageComponentId,
                        principalTable: "InvestmentCostPackageComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentCostPackageAssets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InvestmentCostPackageComponentId = table.Column<Guid>(type: "uuid", nullable: false),
                    DevicesAndAssetsUHIAId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    YearlyDepreciationPercentage = table.Column<double>(type: "double precision", nullable: true),
                    YearlyMaintenancePercentage = table.Column<double>(type: "double precision", nullable: false),
                    TotalCost = table.Column<double>(type: "double precision", nullable: true),
                    YearlyDepreciationCostForTheAddedAssets = table.Column<double>(type: "double precision", nullable: true),
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
                    table.PrimaryKey("PK_InvestmentCostPackageAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentCostPackageAssets_DevicesAndAssetsUHIA_DevicesAnd~",
                        column: x => x.DevicesAndAssetsUHIAId,
                        principalTable: "DevicesAndAssetsUHIA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentCostPackageAssets_InvestmentCostPackageComponents~",
                        column: x => x.InvestmentCostPackageComponentId,
                        principalTable: "InvestmentCostPackageComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentCostDepreciationsAndMaintenances_InvestmentCostPa~",
                table: "InvestmentCostDepreciationsAndMaintenances",
                column: "InvestmentCostPackageComponentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentCostPackageAssets_DevicesAndAssetsUHIAId",
                table: "InvestmentCostPackageAssets",
                column: "DevicesAndAssetsUHIAId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentCostPackageAssets_InvestmentCostPackageComponentId",
                table: "InvestmentCostPackageAssets",
                column: "InvestmentCostPackageComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentCostPackageComponents_FacilityUHIAId",
                table: "InvestmentCostPackageComponents",
                column: "FacilityUHIAId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentCostPackageComponents_PackageHeaderId",
                table: "InvestmentCostPackageComponents",
                column: "PackageHeaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvestmentCostDepreciationsAndMaintenances");

            migrationBuilder.DropTable(
                name: "InvestmentCostPackageAssets");

            migrationBuilder.DropTable(
                name: "InvestmentCostPackageComponents");
        }
    }
}
