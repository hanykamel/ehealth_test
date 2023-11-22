using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHealth.ManageItemLists.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTableNameFromConsumablesAndDevicesToConsumablesAndDevicesUHIA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemListPrices_ConsumablesAndDevices_ConsumablesAndDevicesId",
                table: "ItemListPrices");

            migrationBuilder.DropTable(
                name: "ConsumablesAndDevices");

            migrationBuilder.RenameColumn(
                name: "ConsumablesAndDevicesId",
                table: "ItemListPrices",
                newName: "ConsumablesAndDevicesUHIAId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemListPrices_ConsumablesAndDevicesId",
                table: "ItemListPrices",
                newName: "IX_ItemListPrices_ConsumablesAndDevicesUHIAId");

            migrationBuilder.CreateTable(
                name: "ConsumablesAndDevicesUHIA",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EHealthCode = table.Column<string>(type: "text", nullable: false),
                    UHIAId = table.Column<string>(type: "text", nullable: false),
                    ShortDescriptorAr = table.Column<string>(type: "text", nullable: true),
                    ShortDescriptorEn = table.Column<string>(type: "text", nullable: false),
                    UnitOfMeasureId = table.Column<int>(type: "integer", nullable: false),
                    ServiceCategoryId = table.Column<int>(type: "integer", nullable: false),
                    SubCategoryId = table.Column<int>(type: "integer", nullable: false),
                    ItemListId = table.Column<int>(type: "integer", nullable: false),
                    DataEffectiveDateFrom = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataEffectiveDateTo = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                    table.PrimaryKey("PK_ConsumablesAndDevicesUHIA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsumablesAndDevicesUHIA_Categories_ServiceCategoryId",
                        column: x => x.ServiceCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsumablesAndDevicesUHIA_ItemLists_ItemListId",
                        column: x => x.ItemListId,
                        principalTable: "ItemLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsumablesAndDevicesUHIA_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsumablesAndDevicesUHIA_UnitOfMeasures_UnitOfMeasureId",
                        column: x => x.UnitOfMeasureId,
                        principalTable: "UnitOfMeasures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsumablesAndDevicesUHIA_ItemListId",
                table: "ConsumablesAndDevicesUHIA",
                column: "ItemListId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumablesAndDevicesUHIA_ServiceCategoryId",
                table: "ConsumablesAndDevicesUHIA",
                column: "ServiceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumablesAndDevicesUHIA_SubCategoryId",
                table: "ConsumablesAndDevicesUHIA",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumablesAndDevicesUHIA_UnitOfMeasureId",
                table: "ConsumablesAndDevicesUHIA",
                column: "UnitOfMeasureId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemListPrices_ConsumablesAndDevicesUHIA_ConsumablesAndDevi~",
                table: "ItemListPrices",
                column: "ConsumablesAndDevicesUHIAId",
                principalTable: "ConsumablesAndDevicesUHIA",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemListPrices_ConsumablesAndDevicesUHIA_ConsumablesAndDevi~",
                table: "ItemListPrices");

            migrationBuilder.DropTable(
                name: "ConsumablesAndDevicesUHIA");

            migrationBuilder.RenameColumn(
                name: "ConsumablesAndDevicesUHIAId",
                table: "ItemListPrices",
                newName: "ConsumablesAndDevicesId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemListPrices_ConsumablesAndDevicesUHIAId",
                table: "ItemListPrices",
                newName: "IX_ItemListPrices_ConsumablesAndDevicesId");

            migrationBuilder.CreateTable(
                name: "ConsumablesAndDevices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemListId = table.Column<int>(type: "integer", nullable: false),
                    ServiceCategoryId = table.Column<int>(type: "integer", nullable: false),
                    SubCategoryId = table.Column<int>(type: "integer", nullable: false),
                    UnitOfMeasureId = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DataEffectiveDateFrom = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataEffectiveDateTo = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EHealthCode = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsDeletedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ShortDescriptorAr = table.Column<string>(type: "text", nullable: true),
                    ShortDescriptorEn = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    UHIAId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumablesAndDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsumablesAndDevices_Categories_ServiceCategoryId",
                        column: x => x.ServiceCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsumablesAndDevices_ItemLists_ItemListId",
                        column: x => x.ItemListId,
                        principalTable: "ItemLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsumablesAndDevices_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsumablesAndDevices_UnitOfMeasures_UnitOfMeasureId",
                        column: x => x.UnitOfMeasureId,
                        principalTable: "UnitOfMeasures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsumablesAndDevices_ItemListId",
                table: "ConsumablesAndDevices",
                column: "ItemListId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumablesAndDevices_ServiceCategoryId",
                table: "ConsumablesAndDevices",
                column: "ServiceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumablesAndDevices_SubCategoryId",
                table: "ConsumablesAndDevices",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumablesAndDevices_UnitOfMeasureId",
                table: "ConsumablesAndDevices",
                column: "UnitOfMeasureId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemListPrices_ConsumablesAndDevices_ConsumablesAndDevicesId",
                table: "ItemListPrices",
                column: "ConsumablesAndDevicesId",
                principalTable: "ConsumablesAndDevices",
                principalColumn: "Id");
        }
    }
}
