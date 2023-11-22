using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EHealth.ManageItemLists.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addresourceUHIA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ItemListPrices",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.CreateTable(
                name: "PriceUnits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameAr = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NameEN = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DefinitionAr = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    DefinitionEN = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
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
                    table.PrimaryKey("PK_PriceUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResourceUHIA",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    DescriptorAr = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DescriptorEn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
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
                    table.PrimaryKey("PK_ResourceUHIA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceUHIA_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourceUHIA_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResourceItemPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Price = table.Column<double>(type: "double precision", precision: 11, scale: 7, nullable: false),
                    PriceUnitId = table.Column<int>(type: "integer", nullable: false),
                    EffectiveDateFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EffectiveDateTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResourceUHIAId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsDeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceItemPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceItemPrices_PriceUnits_PriceUnitId",
                        column: x => x.PriceUnitId,
                        principalTable: "PriceUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourceItemPrices_ResourceUHIA_ResourceUHIAId",
                        column: x => x.ResourceUHIAId,
                        principalTable: "ResourceUHIA",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResourceItemPrices_PriceUnitId",
                table: "ResourceItemPrices",
                column: "PriceUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceItemPrices_ResourceUHIAId",
                table: "ResourceItemPrices",
                column: "ResourceUHIAId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceUHIA_CategoryId",
                table: "ResourceUHIA",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceUHIA_SubCategoryId",
                table: "ResourceUHIA",
                column: "SubCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResourceItemPrices");

            migrationBuilder.DropTable(
                name: "PriceUnits");

            migrationBuilder.DropTable(
                name: "ResourceUHIA");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ItemListPrices",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);
        }
    }
}
