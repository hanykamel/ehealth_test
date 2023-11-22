using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHealth.ManageItemLists.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class service_UHIA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ServiceUHIAId",
                table: "ItemListPrices",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ServicesUHIA",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    ShortDescAr = table.Column<string>(type: "text", nullable: true),
                    LongDescAr = table.Column<string>(type: "text", nullable: true),
                    ShortDescEn = table.Column<string>(type: "text", nullable: true),
                    LongDescEn = table.Column<string>(type: "text", nullable: true),
                    FullDescriptorAr = table.Column<string>(type: "text", nullable: true),
                    FullDescriptorEn = table.Column<string>(type: "text", nullable: false),
                    ServiceCategoryId = table.Column<int>(type: "integer", nullable: false),
                    ServiceSubCategoryId = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_ServicesUHIA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicesUHIA_Categories_ServiceCategoryId",
                        column: x => x.ServiceCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServicesUHIA_ItemLists_ItemListId",
                        column: x => x.ItemListId,
                        principalTable: "ItemLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServicesUHIA_SubCategories_ServiceSubCategoryId",
                        column: x => x.ServiceSubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemListPrices_ServiceUHIAId",
                table: "ItemListPrices",
                column: "ServiceUHIAId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicesUHIA_ItemListId",
                table: "ServicesUHIA",
                column: "ItemListId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicesUHIA_ServiceCategoryId",
                table: "ServicesUHIA",
                column: "ServiceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicesUHIA_ServiceSubCategoryId",
                table: "ServicesUHIA",
                column: "ServiceSubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemListPrices_ServicesUHIA_ServiceUHIAId",
                table: "ItemListPrices",
                column: "ServiceUHIAId",
                principalTable: "ServicesUHIA",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemListPrices_ServicesUHIA_ServiceUHIAId",
                table: "ItemListPrices");

            migrationBuilder.DropTable(
                name: "ServicesUHIA");

            migrationBuilder.DropIndex(
                name: "IX_ItemListPrices_ServiceUHIAId",
                table: "ItemListPrices");

            migrationBuilder.DropColumn(
                name: "ServiceUHIAId",
                table: "ItemListPrices");
        }
    }
}
