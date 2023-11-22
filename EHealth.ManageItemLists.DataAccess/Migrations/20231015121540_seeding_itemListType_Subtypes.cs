using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHealth.ManageItemLists.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class seeding_itemListType_Subtypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "ItemListTypes",
            columns: new[] { "Id", "NameAr", "NameEN", "CreatedOn", "ModifiedOn", "Code", "IsDeleted" },
            values: new object[,]
            {
                { 1,"خدمات","Services",DateTimeOffset.Now,DateTimeOffset.Now,"SER",false },
                { 2,"الموادالاستهلاكية والأجهزة","Consumables & Devices",DateTimeOffset.Now,DateTimeOffset.Now,"CONS_DEV", false},
                { 3,"إجراء","Procedure",DateTimeOffset.Now,DateTimeOffset.Now,"PROC", false},
                { 4,"الادويه","Drugs",DateTimeOffset.Now,DateTimeOffset.Now,"DRUGS",false},
                { 5,"الأجهزة والأصول","Devices & Assets",DateTimeOffset.Now,DateTimeOffset.Now,"ASSET",false},
                { 6,"منشأة","Facility",DateTimeOffset.Now,DateTimeOffset.Now,"FACILITY",false},
                { 7,"الموارد","Resource",DateTimeOffset.Now,DateTimeOffset.Now,"RESOURCE",false},
                { 8,"رسوم الطبيب","Doctor's Fees",DateTimeOffset.Now,DateTimeOffset.Now,"DOCTOR_FEES",false},

            }
           );

            migrationBuilder.InsertData(
            table: "ItemListSubtypes",
            columns: new[] { "Id", "NameAr", "NameEN", "ItemListTypeId", "CreatedOn", "ModifiedOn", "Code", "IsDeleted" },
            values: new object[,]
            {
                { 1,"اساسي","Basic",1,DateTimeOffset.Now,DateTimeOffset.Now,"SER", false },
                { 2,"UHIA","UHIA",1,DateTimeOffset.Now,DateTimeOffset.Now,"SER_UHIA" , false},
                { 3,"CPT","CPT",1,DateTimeOffset.Now,DateTimeOffset.Now,"SER_CPT", false },
                { 4,"اساسي","Basic",2,DateTimeOffset.Now,DateTimeOffset.Now,"CONS_DEV", false },
                { 5,"UHIA","UHIA",2,DateTimeOffset.Now,DateTimeOffset.Now,"CONS_DEV_UHIA" , false},
                { 6,"HCPCS","HCPCS",2,DateTimeOffset.Now,DateTimeOffset.Now,"CONS_DEV_HCPCS", false },
                { 7,"اساسي","Basic",3,DateTimeOffset.Now,DateTimeOffset.Now,"PROC" , false},
                { 8,"ICHI","ICHI",3,DateTimeOffset.Now,DateTimeOffset.Now,"PRO_ICHI", false },
                { 9,"CPT","CPT",3,DateTimeOffset.Now,DateTimeOffset.Now,"PRO_CPT", false },
                { 10,"ACHI","ACHI",3,DateTimeOffset.Now,DateTimeOffset.Now,"PRO_ACHI", false },
                { 11,"اساسي","Basic",4,DateTimeOffset.Now,DateTimeOffset.Now,"DRUGS" , false},
                { 12,"UHIA","UHIA",4,DateTimeOffset.Now,DateTimeOffset.Now,"DRUGS_UHIA" , false },
                { 13,"اساسي","Basic",5,DateTimeOffset.Now,DateTimeOffset.Now,"ASSET", false },
                { 14,"UHIA","UHIA",5,DateTimeOffset.Now,DateTimeOffset.Now,"ASSET_UHIA" , false},
                { 15,"اساسي","Basic",6,DateTimeOffset.Now,DateTimeOffset.Now,"FACILITY", false },
                { 16,"UHIA","UHIA",6,DateTimeOffset.Now,DateTimeOffset.Now,"FACILITY_UHIA", false },
                { 17,"اساسي","Basic",7,DateTimeOffset.Now,DateTimeOffset.Now,"RESOURCE" , false},
                { 18,"UHIA","UHIA",7,DateTimeOffset.Now,DateTimeOffset.Now,"RESOURCE_UHIA" , false},
                { 19,"اساسي","Basic",8,DateTimeOffset.Now,DateTimeOffset.Now,"DOCTOR_FEES" , false},
                { 20,"UHIA","UHIA",8,DateTimeOffset.Now,DateTimeOffset.Now,"DOCTOR_FEES_UHIA" , false},



            

            }
           );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE From \"ItemListSubtypes\" WHERE \"Id\" < 21;");
            migrationBuilder.Sql("DELETE From \"ItemListTypes\" WHERE \"Id\" < 9;");
        }
    }
}
