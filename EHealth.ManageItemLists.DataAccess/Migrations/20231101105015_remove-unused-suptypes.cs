using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHealth.ManageItemLists.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class removeunusedsuptypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.UpdateData(
            table: "ItemListSubtypes",
            keyColumn: "Id",
            keyValue: 1,
            column: "IsDeleted",
            value: true);

            migrationBuilder.UpdateData(
            table: "ItemListSubtypes",
            keyColumn: "Id",
            keyValue: 3,
            column: "IsDeleted",
            value: true);

            migrationBuilder.UpdateData(
            table: "ItemListSubtypes",
            keyColumn: "Id",
            keyValue: 4,
            column: "IsDeleted",
            value: true);

            migrationBuilder.UpdateData(
            table: "ItemListSubtypes",
            keyColumn: "Id",
            keyValue: 6,
            column: "IsDeleted",
            value: true);

            migrationBuilder.UpdateData(
           table: "ItemListSubtypes",
           keyColumn: "Id",
           keyValue: 7,
           column: "IsDeleted",
           value: true);

            migrationBuilder.UpdateData(
           table: "ItemListSubtypes",
           keyColumn: "Id",
           keyValue: 9,
           column: "IsDeleted",
           value: true);

            migrationBuilder.UpdateData(
            table: "ItemListSubtypes",
            keyColumn: "Id",
            keyValue: 10,
            column: "IsDeleted",
            value: true);

            migrationBuilder.UpdateData(
           table: "ItemListSubtypes",
           keyColumn: "Id",
           keyValue: 11,
           column: "IsDeleted",
            value: true);

            migrationBuilder.UpdateData(
           table: "ItemListSubtypes",
           keyColumn: "Id",
           keyValue: 13,
           column: "IsDeleted",
            value: true);

            migrationBuilder.UpdateData(
           table: "ItemListSubtypes",
           keyColumn: "Id",
           keyValue: 15,
           column: "IsDeleted",
            value: true);

            migrationBuilder.UpdateData(
           table: "ItemListSubtypes",
           keyColumn: "Id",
           keyValue: 17,
           column: "IsDeleted",
            value: true);

            migrationBuilder.UpdateData(
         table: "ItemListSubtypes",
         keyColumn: "Id",
         keyValue: 19,
         column: "IsDeleted",
            value: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {






        }
    }
}
