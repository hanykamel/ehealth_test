using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EHealth.ManageItemLists.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addPackageHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResourceUnitOfCostValue",
                table: "PriceUnits",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GlobelPackageTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GlobalTypeAr = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    GlobalTypeEn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
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
                    table.PrimaryKey("PK_GlobelPackageTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PackageSpecialties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SpecialtyAr = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SpecialtyEn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
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
                    table.PrimaryKey("PK_PackageSpecialties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PackageTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameAr = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NameEN = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
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
                    table.PrimaryKey("PK_PackageTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PackageSubTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameAr = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NameEN = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PackageTypeId = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_PackageSubTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageSubTypes_PackageTypes_PackageTypeId",
                        column: x => x.PackageTypeId,
                        principalTable: "PackageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PackageHeaders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EHealthCode = table.Column<string>(type: "text", nullable: true),
                    UHIACode = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    NameAr = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    PackageTypeId = table.Column<int>(type: "integer", nullable: false),
                    PackageSubTypeId = table.Column<int>(type: "integer", nullable: false),
                    PackageComplexityClassificationId = table.Column<int>(type: "integer", nullable: true),
                    GlobelPackageTypeId = table.Column<int>(type: "integer", nullable: false),
                    PackageSpecialtyId = table.Column<int>(type: "integer", nullable: false),
                    PackageDuration = table.Column<int>(type: "integer", nullable: false),
                    ActivationDateFrom = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ActivationDateTo = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    PackagePrice = table.Column<double>(type: "double precision", nullable: false),
                    PackageRoundPrice = table.Column<double>(type: "double precision", nullable: false),
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
                    table.PrimaryKey("PK_PackageHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageHeaders_GlobelPackageTypes_GlobelPackageTypeId",
                        column: x => x.GlobelPackageTypeId,
                        principalTable: "GlobelPackageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageHeaders_PackageComplexityClassifications_PackageComp~",
                        column: x => x.PackageComplexityClassificationId,
                        principalTable: "PackageComplexityClassifications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PackageHeaders_PackageSpecialties_PackageSpecialtyId",
                        column: x => x.PackageSpecialtyId,
                        principalTable: "PackageSpecialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageHeaders_PackageSubTypes_PackageSubTypeId",
                        column: x => x.PackageSubTypeId,
                        principalTable: "PackageSubTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageHeaders_PackageTypes_PackageTypeId",
                        column: x => x.PackageTypeId,
                        principalTable: "PackageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PackageHeaders_GlobelPackageTypeId",
                table: "PackageHeaders",
                column: "GlobelPackageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageHeaders_PackageComplexityClassificationId",
                table: "PackageHeaders",
                column: "PackageComplexityClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageHeaders_PackageSpecialtyId",
                table: "PackageHeaders",
                column: "PackageSpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageHeaders_PackageSubTypeId",
                table: "PackageHeaders",
                column: "PackageSubTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageHeaders_PackageTypeId",
                table: "PackageHeaders",
                column: "PackageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageSubTypes_PackageTypeId",
                table: "PackageSubTypes",
                column: "PackageTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackageHeaders");

            migrationBuilder.DropTable(
                name: "GlobelPackageTypes");

            migrationBuilder.DropTable(
                name: "PackageSpecialties");

            migrationBuilder.DropTable(
                name: "PackageSubTypes");

            migrationBuilder.DropTable(
                name: "PackageTypes");

            migrationBuilder.DropColumn(
                name: "ResourceUnitOfCostValue",
                table: "PriceUnits");
        }
    }
}
