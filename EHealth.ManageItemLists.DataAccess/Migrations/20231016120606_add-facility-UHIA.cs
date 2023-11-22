using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHealth.ManageItemLists.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addfacilityUHIA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FacilityUHIA",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    DescriptorAr = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DescriptorEn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OccupancyRate = table.Column<double>(type: "double precision", precision: 11, scale: 7, nullable: true),
                    OperatingRateInHoursPerDay = table.Column<double>(type: "double precision", precision: 11, scale: 7, nullable: false),
                    OperatingDaysPerMonth = table.Column<double>(type: "double precision", precision: 11, scale: 7, nullable: false),
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
                    table.PrimaryKey("PK_FacilityUHIA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacilityUHIA_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacilityUHIA_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacilityUHIA_CategoryId",
                table: "FacilityUHIA",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityUHIA_SubCategoryId",
                table: "FacilityUHIA",
                column: "SubCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FacilityUHIA");
        }
    }
}
