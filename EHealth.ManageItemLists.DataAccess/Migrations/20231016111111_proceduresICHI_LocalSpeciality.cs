using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EHealth.ManageItemLists.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class proceduresICHI_LocalSpeciality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullDescriptorAr",
                table: "ServicesUHIA");

            migrationBuilder.DropColumn(
                name: "LongDescAr",
                table: "ServicesUHIA");

            migrationBuilder.DropColumn(
                name: "LongDescEn",
                table: "ServicesUHIA");

            migrationBuilder.DropColumn(
                name: "FullDescriptorAr",
                table: "ConsumablesAndDevices");

            migrationBuilder.DropColumn(
                name: "FullDescriptorEn",
                table: "ConsumablesAndDevices");

            migrationBuilder.DropColumn(
                name: "LongDescriptorAr",
                table: "ConsumablesAndDevices");

            migrationBuilder.DropColumn(
                name: "LongDescriptorEn",
                table: "ConsumablesAndDevices");

            migrationBuilder.RenameColumn(
                name: "FullDescriptorEn",
                table: "ServicesUHIA",
                newName: "UHIAId");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "ServicesUHIA",
                newName: "EHealthCode");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "ConsumablesAndDevices",
                newName: "UHIAId");

            migrationBuilder.AddColumn<Guid>(
                name: "ProcedureICHIId",
                table: "ItemListPrices",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EHealthCode",
                table: "ConsumablesAndDevices",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "LocalSpecialtyDepartments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LocalSpecialityAr = table.Column<string>(type: "text", nullable: false),
                    LocalSpecialityENG = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_LocalSpecialtyDepartments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProceduresICHI",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EHealthCode = table.Column<string>(type: "text", nullable: false),
                    UHIAId = table.Column<string>(type: "text", nullable: false),
                    TitleAr = table.Column<string>(type: "text", nullable: true),
                    TitleEn = table.Column<string>(type: "text", nullable: false),
                    ServiceCategoryId = table.Column<int>(type: "integer", nullable: false),
                    SubCategoryId = table.Column<int>(type: "integer", nullable: false),
                    ItemListId = table.Column<int>(type: "integer", nullable: false),
                    LocalSpecialtyDepartmentId = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_ProceduresICHI", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProceduresICHI_Categories_ServiceCategoryId",
                        column: x => x.ServiceCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProceduresICHI_ItemLists_ItemListId",
                        column: x => x.ItemListId,
                        principalTable: "ItemLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProceduresICHI_LocalSpecialtyDepartments_LocalSpecialtyDepa~",
                        column: x => x.LocalSpecialtyDepartmentId,
                        principalTable: "LocalSpecialtyDepartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProceduresICHI_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemListPrices_ProcedureICHIId",
                table: "ItemListPrices",
                column: "ProcedureICHIId");

            migrationBuilder.CreateIndex(
                name: "IX_ProceduresICHI_ItemListId",
                table: "ProceduresICHI",
                column: "ItemListId");

            migrationBuilder.CreateIndex(
                name: "IX_ProceduresICHI_LocalSpecialtyDepartmentId",
                table: "ProceduresICHI",
                column: "LocalSpecialtyDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProceduresICHI_ServiceCategoryId",
                table: "ProceduresICHI",
                column: "ServiceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProceduresICHI_SubCategoryId",
                table: "ProceduresICHI",
                column: "SubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemListPrices_ProceduresICHI_ProcedureICHIId",
                table: "ItemListPrices",
                column: "ProcedureICHIId",
                principalTable: "ProceduresICHI",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemListPrices_ProceduresICHI_ProcedureICHIId",
                table: "ItemListPrices");

            migrationBuilder.DropTable(
                name: "ProceduresICHI");

            migrationBuilder.DropTable(
                name: "LocalSpecialtyDepartments");

            migrationBuilder.DropIndex(
                name: "IX_ItemListPrices_ProcedureICHIId",
                table: "ItemListPrices");

            migrationBuilder.DropColumn(
                name: "ProcedureICHIId",
                table: "ItemListPrices");

            migrationBuilder.DropColumn(
                name: "EHealthCode",
                table: "ConsumablesAndDevices");

            migrationBuilder.RenameColumn(
                name: "UHIAId",
                table: "ServicesUHIA",
                newName: "FullDescriptorEn");

            migrationBuilder.RenameColumn(
                name: "EHealthCode",
                table: "ServicesUHIA",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "UHIAId",
                table: "ConsumablesAndDevices",
                newName: "Code");

            migrationBuilder.AddColumn<string>(
                name: "FullDescriptorAr",
                table: "ServicesUHIA",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LongDescAr",
                table: "ServicesUHIA",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LongDescEn",
                table: "ServicesUHIA",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullDescriptorAr",
                table: "ConsumablesAndDevices",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullDescriptorEn",
                table: "ConsumablesAndDevices",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LongDescriptorAr",
                table: "ConsumablesAndDevices",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LongDescriptorEn",
                table: "ConsumablesAndDevices",
                type: "text",
                nullable: true);
        }
    }
}
