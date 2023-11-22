using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EHealth.ManageItemLists.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class consumablesAndDevices_UnitOdMeasure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ConsumablesAndDevicesId",
                table: "ItemListPrices",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UnitOfMeasures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MeasureTypeAr = table.Column<string>(type: "text", nullable: false),
                    MeasureTypeENG = table.Column<string>(type: "text", nullable: false),
                    DefinitionAr = table.Column<string>(type: "text", nullable: true),
                    DefinitionENG = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsDeletedBy = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitOfMeasures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConsumablesAndDevices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    ShortDescriptorAr = table.Column<string>(type: "text", nullable: true),
                    ShortDescriptorEn = table.Column<string>(type: "text", nullable: false),
                    LongDescriptorAr = table.Column<string>(type: "text", nullable: true),
                    LongDescriptorEn = table.Column<string>(type: "text", nullable: true),
                    FullDescriptorAr = table.Column<string>(type: "text", nullable: true),
                    FullDescriptorEn = table.Column<string>(type: "text", nullable: true),
                    UnitOfMeasureId = table.Column<int>(type: "integer", nullable: false),
                    ServiceCategoryId = table.Column<int>(type: "integer", nullable: false),
                    SubCategoryId = table.Column<int>(type: "integer", nullable: false),
                    ItemListId = table.Column<int>(type: "integer", nullable: false),
                    DataEffectiveDateFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataEffectiveDateTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsDeletedBy = table.Column<string>(type: "text", nullable: true)
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
                name: "IX_ItemListPrices_ConsumablesAndDevicesId",
                table: "ItemListPrices",
                column: "ConsumablesAndDevicesId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemListPrices_ConsumablesAndDevices_ConsumablesAndDevicesId",
                table: "ItemListPrices");

            migrationBuilder.DropTable(
                name: "ConsumablesAndDevices");

            migrationBuilder.DropTable(
                name: "UnitOfMeasures");

            migrationBuilder.DropIndex(
                name: "IX_ItemListPrices_ConsumablesAndDevicesId",
                table: "ItemListPrices");

            migrationBuilder.DropColumn(
                name: "ConsumablesAndDevicesId",
                table: "ItemListPrices");
        }
    }
}
