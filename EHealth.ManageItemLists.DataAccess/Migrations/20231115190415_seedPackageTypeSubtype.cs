using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHealth.ManageItemLists.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class seedPackageTypeSubtype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "PackageTypes",
            columns: new[] { "Id", "NameAr", "NameEN", "CreatedOn", "TenantId", "Code" },
            values: new object[,]
            {
                { 1,"صفقة منفعة","Benefit Package",DateTimeOffset.Now,"EHealth","Benefit Package"},
                { 2,"صفقة تكلفة","Cost Package",DateTimeOffset.Now, "EHealth","Cost Package"},
            }
           );


            migrationBuilder.InsertData(
            table: "PackageSubTypes",
            columns: new[] { "Id", "NameAr", "NameEN", "PackageTypeId", "CreatedOn", "TenantId", "Code" },
            values: new object[,]
            {
                { 1,"جراحة","Surgical",1,DateTimeOffset.Now, "EHealth","Surgical" },
                { 2,"زرع اعضاء","Transplant",1,DateTimeOffset.Now, "EHealth","Transplant"},
                { 3,"قسطرة","Catheterization",1,DateTimeOffset.Now, "EHealth","Catheterization"},
                { 4,"منظار","Endoscopic",1,DateTimeOffset.Now, "EHealth","Endoscopic"},
                { 5,"الاشعة التداخلية","Interventional radiology",1,DateTimeOffset.Now, "EHealth","Interventional radiology"},
                { 6,"غسيل الكلي","Renal Dialysis",1,DateTimeOffset.Now, "EHealth","Renal Dialysis"},
                { 7,"اسنان","Dental",1,DateTimeOffset.Now, "EHealth","Dental"},
                { 8,"تكلفة استثمارية","Investment Cost",2,DateTimeOffset.Now, "EHealth","Investment Cost"},
                { 9,"اجر الطبيب","Doctor’s fees",2,DateTimeOffset.Now, "EHealth","Doctor’s fees"},
                { 10,"أجور الموارد","Fees of Resource per unit",2,DateTimeOffset.Now, "EHealth","Fees of Resource per unit"},
                { 11,"دقيقة الاسنان","Dental Minute",2,DateTimeOffset.Now, "EHealth","Dental Minute"},
                { 12,"رسوم دقيقة العمليات","OR Opening Fees",2,DateTimeOffset.Now, "EHealth","OR Opening Fees"},
                { 13,"البنود المشتركة","Shared Item",2,DateTimeOffset.Now, "EHealth","Shared Item"}
            }
           );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE From \"PackageTypes\" WHERE \"Id\" < 2;");
            migrationBuilder.Sql("DELETE From \"PackageSubTypes\" WHERE \"Id\" < 13;");
        }
    }
}
