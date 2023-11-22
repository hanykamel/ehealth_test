using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EHealth.ManageItemLists.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DrugsUHIA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DrugsPackageTypes",
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
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsDeletedBy = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugsPackageTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RegistrationTypeAr = table.Column<string>(type: "text", nullable: false),
                    RegistrationTypeENG = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_RegistrationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReimbursementCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameAr = table.Column<string>(type: "text", nullable: false),
                    NameENG = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_ReimbursementCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnitsTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UnitAr = table.Column<string>(type: "text", nullable: false),
                    UnitEn = table.Column<string>(type: "text", nullable: false),
                    DefinitionAr = table.Column<string>(type: "text", nullable: true),
                    DefinitionEn = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_UnitsTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DrugsUHIA",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EHealthDrugCode = table.Column<string>(type: "text", nullable: true),
                    LocalDrugCode = table.Column<string>(type: "text", nullable: false),
                    InternationalNonProprietaryName = table.Column<string>(type: "text", nullable: true),
                    ProprietaryName = table.Column<string>(type: "text", nullable: false),
                    DosageForm = table.Column<string>(type: "text", nullable: false),
                    RouteOfAdministration = table.Column<string>(type: "text", nullable: true),
                    Manufacturer = table.Column<string>(type: "text", nullable: true),
                    MarketAuthorizationHolder = table.Column<string>(type: "text", nullable: true),
                    RegistrationTypeId = table.Column<int>(type: "integer", nullable: true),
                    DrugsPackageTypeId = table.Column<int>(type: "integer", nullable: true),
                    MainUnitId = table.Column<int>(type: "integer", nullable: true),
                    NumberOfMainUnit = table.Column<int>(type: "integer", nullable: true),
                    SubUnitId = table.Column<int>(type: "integer", nullable: false),
                    NumberOfSubunitPerMainUnit = table.Column<int>(type: "integer", nullable: true),
                    TotalNumberSubunitsOfPack = table.Column<int>(type: "integer", nullable: true),
                    ReimbursementCategoryId = table.Column<int>(type: "integer", nullable: true),
                    DataEffectiveDateFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataEffectiveDateTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ItemListId = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsDeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugsUHIA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrugsUHIA_DrugsPackageTypes_DrugsPackageTypeId",
                        column: x => x.DrugsPackageTypeId,
                        principalTable: "DrugsPackageTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DrugsUHIA_ItemLists_ItemListId",
                        column: x => x.ItemListId,
                        principalTable: "ItemLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrugsUHIA_RegistrationTypes_RegistrationTypeId",
                        column: x => x.RegistrationTypeId,
                        principalTable: "RegistrationTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DrugsUHIA_ReimbursementCategories_ReimbursementCategoryId",
                        column: x => x.ReimbursementCategoryId,
                        principalTable: "ReimbursementCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DrugsUHIA_UnitsTypes_MainUnitId",
                        column: x => x.MainUnitId,
                        principalTable: "UnitsTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DrugsUHIA_UnitsTypes_SubUnitId",
                        column: x => x.SubUnitId,
                        principalTable: "UnitsTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DrugsPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MainUnitPrice = table.Column<double>(type: "double precision", nullable: false),
                    FullPackPrice = table.Column<double>(type: "double precision", nullable: false),
                    SubUnitPrice = table.Column<double>(type: "double precision", nullable: false),
                    EffectiveDateFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EffectiveDateTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DrugUHIAId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsDeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugsPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrugsPrices_DrugsUHIA_DrugUHIAId",
                        column: x => x.DrugUHIAId,
                        principalTable: "DrugsUHIA",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrugsPrices_DrugUHIAId",
                table: "DrugsPrices",
                column: "DrugUHIAId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugsUHIA_DrugsPackageTypeId",
                table: "DrugsUHIA",
                column: "DrugsPackageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugsUHIA_ItemListId",
                table: "DrugsUHIA",
                column: "ItemListId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugsUHIA_MainUnitId",
                table: "DrugsUHIA",
                column: "MainUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugsUHIA_RegistrationTypeId",
                table: "DrugsUHIA",
                column: "RegistrationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugsUHIA_ReimbursementCategoryId",
                table: "DrugsUHIA",
                column: "ReimbursementCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugsUHIA_SubUnitId",
                table: "DrugsUHIA",
                column: "SubUnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrugsPrices");

            migrationBuilder.DropTable(
                name: "DrugsUHIA");

            migrationBuilder.DropTable(
                name: "DrugsPackageTypes");

            migrationBuilder.DropTable(
                name: "RegistrationTypes");

            migrationBuilder.DropTable(
                name: "ReimbursementCategories");

            migrationBuilder.DropTable(
                name: "UnitsTypes");
        }
    }
}
