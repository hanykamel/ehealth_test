using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHealth.ManageItemLists.DataAccess.Migrations
{
    public partial class makeFacilityNullableInInvestmentCostPackageAndFeesOfResourcesPackage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeesOfResourcesPerUnitPackageComponents_FacilityUHIA_Facili~",
                table: "FeesOfResourcesPerUnitPackageComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentCostPackageComponents_FacilityUHIA_FacilityUHIAId",
                table: "InvestmentCostPackageComponents");

            migrationBuilder.AlterColumn<Guid>(
                name: "FacilityUHIAId",
                table: "InvestmentCostPackageComponents",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "FacilityUHIAId",
                table: "FeesOfResourcesPerUnitPackageComponents",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_FeesOfResourcesPerUnitPackageComponents_FacilityUHIA_Facili~",
                table: "FeesOfResourcesPerUnitPackageComponents",
                column: "FacilityUHIAId",
                principalTable: "FacilityUHIA",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentCostPackageComponents_FacilityUHIA_FacilityUHIAId",
                table: "InvestmentCostPackageComponents",
                column: "FacilityUHIAId",
                principalTable: "FacilityUHIA",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeesOfResourcesPerUnitPackageComponents_FacilityUHIA_Facili~",
                table: "FeesOfResourcesPerUnitPackageComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentCostPackageComponents_FacilityUHIA_FacilityUHIAId",
                table: "InvestmentCostPackageComponents");

            migrationBuilder.AlterColumn<Guid>(
                name: "FacilityUHIAId",
                table: "InvestmentCostPackageComponents",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "FacilityUHIAId",
                table: "FeesOfResourcesPerUnitPackageComponents",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FeesOfResourcesPerUnitPackageComponents_FacilityUHIA_Facili~",
                table: "FeesOfResourcesPerUnitPackageComponents",
                column: "FacilityUHIAId",
                principalTable: "FacilityUHIA",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentCostPackageComponents_FacilityUHIA_FacilityUHIAId",
                table: "InvestmentCostPackageComponents",
                column: "FacilityUHIAId",
                principalTable: "FacilityUHIA",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
