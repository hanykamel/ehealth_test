using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHealth.ManageItemLists.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class change_LocalSpecialtyDepartment_field_tobe_nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProceduresICHI_LocalSpecialtyDepartments_LocalSpecialtyDepa~",
                table: "ProceduresICHI");

            migrationBuilder.AlterColumn<int>(
                name: "LocalSpecialtyDepartmentId",
                table: "ProceduresICHI",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_ProceduresICHI_LocalSpecialtyDepartments_LocalSpecialtyDepa~",
                table: "ProceduresICHI",
                column: "LocalSpecialtyDepartmentId",
                principalTable: "LocalSpecialtyDepartments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProceduresICHI_LocalSpecialtyDepartments_LocalSpecialtyDepa~",
                table: "ProceduresICHI");

            migrationBuilder.AlterColumn<int>(
                name: "LocalSpecialtyDepartmentId",
                table: "ProceduresICHI",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProceduresICHI_LocalSpecialtyDepartments_LocalSpecialtyDepa~",
                table: "ProceduresICHI",
                column: "LocalSpecialtyDepartmentId",
                principalTable: "LocalSpecialtyDepartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
