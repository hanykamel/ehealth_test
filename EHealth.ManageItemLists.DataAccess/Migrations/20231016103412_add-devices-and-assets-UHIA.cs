using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EHealth.ManageItemLists.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class adddevicesandassetsUHIA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DevicesAndAssetsUHIAId",
                table: "ItemListPrices",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UnitRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameAr = table.Column<string>(type: "text", nullable: false),
                    NameEN = table.Column<string>(type: "text", nullable: false),
                    DefinitionAr = table.Column<string>(type: "text", nullable: true),
                    DefinitionEN = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeletedBy = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DevicesAndAssetsUHIA",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    DescriptorAr = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DescriptorEn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UnitRoomId = table.Column<int>(type: "integer", nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    SubCategoryId = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_DevicesAndAssetsUHIA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DevicesAndAssetsUHIA_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DevicesAndAssetsUHIA_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DevicesAndAssetsUHIA_UnitRooms_UnitRoomId",
                        column: x => x.UnitRoomId,
                        principalTable: "UnitRooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemListPrices_DevicesAndAssetsUHIAId",
                table: "ItemListPrices",
                column: "DevicesAndAssetsUHIAId");

            migrationBuilder.CreateIndex(
                name: "IX_DevicesAndAssetsUHIA_CategoryId",
                table: "DevicesAndAssetsUHIA",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DevicesAndAssetsUHIA_SubCategoryId",
                table: "DevicesAndAssetsUHIA",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DevicesAndAssetsUHIA_UnitRoomId",
                table: "DevicesAndAssetsUHIA",
                column: "UnitRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemListPrices_DevicesAndAssetsUHIA_DevicesAndAssetsUHIAId",
                table: "ItemListPrices",
                column: "DevicesAndAssetsUHIAId",
                principalTable: "DevicesAndAssetsUHIA",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemListPrices_DevicesAndAssetsUHIA_DevicesAndAssetsUHIAId",
                table: "ItemListPrices");

            migrationBuilder.DropTable(
                name: "DevicesAndAssetsUHIA");

            migrationBuilder.DropTable(
                name: "UnitRooms");

            migrationBuilder.DropIndex(
                name: "IX_ItemListPrices_DevicesAndAssetsUHIAId",
                table: "ItemListPrices");

            migrationBuilder.DropColumn(
                name: "DevicesAndAssetsUHIAId",
                table: "ItemListPrices");
        }
    }
}
