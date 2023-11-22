using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHealth.ManageItemLists.DataAccess.Migrations
{
    public partial class addFeesOfResourcesPerUnitPackage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeesOfResourcesPerUnitPackageComponents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PackageHeaderId = table.Column<Guid>(type: "uuid", nullable: false),
                    FacilityUHIAId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuantityOfUnitsPerTheFacility = table.Column<int>(type: "integer", nullable: false),
                    NumberOfSessionsPerUnitPerFacility = table.Column<int>(type: "integer", nullable: false),
                    FeesOfResourcesPerUnitPackageSummaryId = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_FeesOfResourcesPerUnitPackageComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeesOfResourcesPerUnitPackageComponents_FacilityUHIA_Facili~",
                        column: x => x.FacilityUHIAId,
                        principalTable: "FacilityUHIA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeesOfResourcesPerUnitPackageComponents_PackageHeaders_Pack~",
                        column: x => x.PackageHeaderId,
                        principalTable: "PackageHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeesOfResourcesPerUnitPackageResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FeesOfResourcesPerUnitPackageComponentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResourceUHIAId = table.Column<Guid>(type: "uuid", nullable: false),
                    DailyCostOfTheResource = table.Column<double>(type: "double precision", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    TotalDailyCostOfResourcePerFacility = table.Column<double>(type: "double precision", nullable: true),
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
                    table.PrimaryKey("PK_FeesOfResourcesPerUnitPackageResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeesOfResourcesPerUnitPackageResources_FeesOfResourcesPerUn~",
                        column: x => x.FeesOfResourcesPerUnitPackageComponentId,
                        principalTable: "FeesOfResourcesPerUnitPackageComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeesOfResourcesPerUnitPackageResources_ResourceUHIA_Resourc~",
                        column: x => x.ResourceUHIAId,
                        principalTable: "ResourceUHIA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeesOfResourcesPerUnitPackageSummaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FeesOfResourcesPerUnitPackageComponentId = table.Column<Guid>(type: "uuid", nullable: false),
                    NumberOfItems = table.Column<int>(type: "integer", nullable: true),
                    TotalNumberOfQuantity = table.Column<int>(type: "integer", nullable: true),
                    DailyCostOfTotalAddedResourcesPerFacility = table.Column<double>(type: "double precision", nullable: true),
                    HourlyCostOfTotalAddedResourcesPerFacility = table.Column<double>(type: "double precision", nullable: true),
                    MinuteCostOfTotalAddedResourcesPerFacility = table.Column<double>(type: "double precision", nullable: true),
                    DailyCostOfTotalAddedResourcesPerUnit = table.Column<double>(type: "double precision", nullable: true),
                    HourlyCostOfTotalAddedResourcesPerUnit = table.Column<double>(type: "double precision", nullable: true),
                    MinuteCostOfTotalAddedResourcesPerUnit = table.Column<double>(type: "double precision", nullable: true),
                    DailyCostOfTotalAddedResourcesPerSession = table.Column<double>(type: "double precision", nullable: true),
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
                    table.PrimaryKey("PK_FeesOfResourcesPerUnitPackageSummaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeesOfResourcesPerUnitPackageSummaries_FeesOfResourcesPerUn~",
                        column: x => x.FeesOfResourcesPerUnitPackageComponentId,
                        principalTable: "FeesOfResourcesPerUnitPackageComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeesOfResourcesPerUnitPackageComponents_FacilityUHIAId",
                table: "FeesOfResourcesPerUnitPackageComponents",
                column: "FacilityUHIAId");

            migrationBuilder.CreateIndex(
                name: "IX_FeesOfResourcesPerUnitPackageComponents_PackageHeaderId",
                table: "FeesOfResourcesPerUnitPackageComponents",
                column: "PackageHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_FeesOfResourcesPerUnitPackageResources_FeesOfResourcesPerUn~",
                table: "FeesOfResourcesPerUnitPackageResources",
                column: "FeesOfResourcesPerUnitPackageComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_FeesOfResourcesPerUnitPackageResources_ResourceUHIAId",
                table: "FeesOfResourcesPerUnitPackageResources",
                column: "ResourceUHIAId");

            migrationBuilder.CreateIndex(
                name: "IX_FeesOfResourcesPerUnitPackageSummaries_FeesOfResourcesPerUn~",
                table: "FeesOfResourcesPerUnitPackageSummaries",
                column: "FeesOfResourcesPerUnitPackageComponentId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeesOfResourcesPerUnitPackageResources");

            migrationBuilder.DropTable(
                name: "FeesOfResourcesPerUnitPackageSummaries");

            migrationBuilder.DropTable(
                name: "FeesOfResourcesPerUnitPackageComponents");
        }
    }
}
