using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EHealth.ManageItemLists.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class adddoctorfeesUHIA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PackageComplexityClassifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ComplexityAr = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ComplexityEn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DefinitionAr = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    DefinitionEn = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
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
                    table.PrimaryKey("PK_PackageComplexityClassifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnitsOfDoctorFees",
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
                    table.PrimaryKey("PK_UnitsOfDoctorFees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DoctorFeesUHIA",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    DescriptorAr = table.Column<string>(type: "text", nullable: true),
                    DescriptorEn = table.Column<string>(type: "text", nullable: false),
                    PackageComplexityClassificationId = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_DoctorFeesUHIA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorFeesUHIA_PackageComplexityClassifications_PackageComp~",
                        column: x => x.PackageComplexityClassificationId,
                        principalTable: "PackageComplexityClassifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctorFeesItemPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DoctorFees = table.Column<double>(type: "double precision", precision: 11, scale: 7, nullable: false),
                    UnitOfDoctorFeesId = table.Column<int>(type: "integer", nullable: false),
                    EffectiveDateFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EffectiveDateTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DoctorFeesUHIAId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsDeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorFeesItemPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorFeesItemPrices_DoctorFeesUHIA_DoctorFeesUHIAId",
                        column: x => x.DoctorFeesUHIAId,
                        principalTable: "DoctorFeesUHIA",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DoctorFeesItemPrices_UnitsOfDoctorFees_UnitOfDoctorFeesId",
                        column: x => x.UnitOfDoctorFeesId,
                        principalTable: "UnitsOfDoctorFees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorFeesItemPrices_DoctorFeesUHIAId",
                table: "DoctorFeesItemPrices",
                column: "DoctorFeesUHIAId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorFeesItemPrices_UnitOfDoctorFeesId",
                table: "DoctorFeesItemPrices",
                column: "UnitOfDoctorFeesId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorFeesUHIA_PackageComplexityClassificationId",
                table: "DoctorFeesUHIA",
                column: "PackageComplexityClassificationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorFeesItemPrices");

            migrationBuilder.DropTable(
                name: "DoctorFeesUHIA");

            migrationBuilder.DropTable(
                name: "UnitsOfDoctorFees");

            migrationBuilder.DropTable(
                name: "PackageComplexityClassifications");
        }
    }
}
