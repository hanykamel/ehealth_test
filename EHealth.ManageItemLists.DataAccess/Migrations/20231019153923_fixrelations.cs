using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHealth.ManageItemLists.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fixrelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemListId",
                table: "ResourceUHIA",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemListId",
                table: "FacilityUHIA",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemListId",
                table: "DoctorFeesUHIA",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemListId",
                table: "DevicesAndAssetsUHIA",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ResourceUHIA_ItemListId",
                table: "ResourceUHIA",
                column: "ItemListId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityUHIA_ItemListId",
                table: "FacilityUHIA",
                column: "ItemListId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorFeesUHIA_ItemListId",
                table: "DoctorFeesUHIA",
                column: "ItemListId");

            migrationBuilder.CreateIndex(
                name: "IX_DevicesAndAssetsUHIA_ItemListId",
                table: "DevicesAndAssetsUHIA",
                column: "ItemListId");

            migrationBuilder.AddForeignKey(
                name: "FK_DevicesAndAssetsUHIA_ItemLists_ItemListId",
                table: "DevicesAndAssetsUHIA",
                column: "ItemListId",
                principalTable: "ItemLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorFeesUHIA_ItemLists_ItemListId",
                table: "DoctorFeesUHIA",
                column: "ItemListId",
                principalTable: "ItemLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityUHIA_ItemLists_ItemListId",
                table: "FacilityUHIA",
                column: "ItemListId",
                principalTable: "ItemLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceUHIA_ItemLists_ItemListId",
                table: "ResourceUHIA",
                column: "ItemListId",
                principalTable: "ItemLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DevicesAndAssetsUHIA_ItemLists_ItemListId",
                table: "DevicesAndAssetsUHIA");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorFeesUHIA_ItemLists_ItemListId",
                table: "DoctorFeesUHIA");

            migrationBuilder.DropForeignKey(
                name: "FK_FacilityUHIA_ItemLists_ItemListId",
                table: "FacilityUHIA");

            migrationBuilder.DropForeignKey(
                name: "FK_ResourceUHIA_ItemLists_ItemListId",
                table: "ResourceUHIA");

            migrationBuilder.DropIndex(
                name: "IX_ResourceUHIA_ItemListId",
                table: "ResourceUHIA");

            migrationBuilder.DropIndex(
                name: "IX_FacilityUHIA_ItemListId",
                table: "FacilityUHIA");

            migrationBuilder.DropIndex(
                name: "IX_DoctorFeesUHIA_ItemListId",
                table: "DoctorFeesUHIA");

            migrationBuilder.DropIndex(
                name: "IX_DevicesAndAssetsUHIA_ItemListId",
                table: "DevicesAndAssetsUHIA");

            migrationBuilder.DropColumn(
                name: "ItemListId",
                table: "ResourceUHIA");

            migrationBuilder.DropColumn(
                name: "ItemListId",
                table: "FacilityUHIA");

            migrationBuilder.DropColumn(
                name: "ItemListId",
                table: "DoctorFeesUHIA");

            migrationBuilder.DropColumn(
                name: "ItemListId",
                table: "DevicesAndAssetsUHIA");
        }
    }
}
