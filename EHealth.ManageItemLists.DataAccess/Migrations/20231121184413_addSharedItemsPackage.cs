using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EHealth.ManageItemLists.DataAccess.Migrations
{
    public partial class addSharedItemsPackage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LocationAr = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LocationEn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DefinitionAr = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    DefinitionEn = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsDeletedBy = table.Column<string>(type: "text", nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SharedItemsPackageComponents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PackageHeaderId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_SharedItemsPackageComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharedItemsPackageComponents_PackageHeaders_PackageHeaderId",
                        column: x => x.PackageHeaderId,
                        principalTable: "PackageHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SharedItemsPackageConsumablesAndDevices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SharedItemsPackageComponentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConsumablesAndDevicesUHIAId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    NumberOfCasesInTheUnit = table.Column<int>(type: "integer", nullable: false),
                    LocationId = table.Column<int>(type: "integer", nullable: true),
                    TotalCost = table.Column<double>(type: "double precision", nullable: true),
                    ConsumablePerCase = table.Column<double>(type: "double precision", nullable: true),
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
                    table.PrimaryKey("PK_SharedItemsPackageConsumablesAndDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharedItemsPackageConsumablesAndDevices_ConsumablesAndDevic~",
                        column: x => x.ConsumablesAndDevicesUHIAId,
                        principalTable: "ConsumablesAndDevicesUHIA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedItemsPackageConsumablesAndDevices_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SharedItemsPackageConsumablesAndDevices_SharedItemsPackageC~",
                        column: x => x.SharedItemsPackageComponentId,
                        principalTable: "SharedItemsPackageComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SharedItemsPackageDrugs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SharedItemsPackageComponentId = table.Column<Guid>(type: "uuid", nullable: false),
                    DrugUHIAId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    NumberOfCasesInTheUnit = table.Column<int>(type: "integer", nullable: false),
                    LocationId = table.Column<int>(type: "integer", nullable: true),
                    TotalCost = table.Column<double>(type: "double precision", nullable: true),
                    DrugPerCase = table.Column<double>(type: "double precision", nullable: true),
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
                    table.PrimaryKey("PK_SharedItemsPackageDrugs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharedItemsPackageDrugs_DrugsUHIA_DrugUHIAId",
                        column: x => x.DrugUHIAId,
                        principalTable: "DrugsUHIA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedItemsPackageDrugs_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SharedItemsPackageDrugs_SharedItemsPackageComponents_Shared~",
                        column: x => x.SharedItemsPackageComponentId,
                        principalTable: "SharedItemsPackageComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SharedItemsPackageComponents_PackageHeaderId",
                table: "SharedItemsPackageComponents",
                column: "PackageHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedItemsPackageConsumablesAndDevices_ConsumablesAndDevic~",
                table: "SharedItemsPackageConsumablesAndDevices",
                column: "ConsumablesAndDevicesUHIAId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedItemsPackageConsumablesAndDevices_LocationId",
                table: "SharedItemsPackageConsumablesAndDevices",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedItemsPackageConsumablesAndDevices_SharedItemsPackageC~",
                table: "SharedItemsPackageConsumablesAndDevices",
                column: "SharedItemsPackageComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedItemsPackageDrugs_DrugUHIAId",
                table: "SharedItemsPackageDrugs",
                column: "DrugUHIAId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedItemsPackageDrugs_LocationId",
                table: "SharedItemsPackageDrugs",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedItemsPackageDrugs_SharedItemsPackageComponentId",
                table: "SharedItemsPackageDrugs",
                column: "SharedItemsPackageComponentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SharedItemsPackageConsumablesAndDevices");

            migrationBuilder.DropTable(
                name: "SharedItemsPackageDrugs");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "SharedItemsPackageComponents");
        }
    }
}
