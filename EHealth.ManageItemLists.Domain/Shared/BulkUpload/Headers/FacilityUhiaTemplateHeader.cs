namespace EHealth.ManageItemLists.Domain.Shared.BulkUpload.Headers
{
    public class FacilityUhiaTemplateHeader
    {
        public static List<HeaderItem> Headers = new List<HeaderItem>
        {
            new HeaderItem{Index=0,Key="EHealthCode",TitleAr="كود أي هيلث",TitleEn="EHealthCode",Lookup=false},
            new HeaderItem{Index=1,Key="DescriptorAr",TitleAr="الوصف عربي",TitleEn="Descriptor (Arabic)", Lookup = false},
            new HeaderItem{Index=2,Key="DescriptorEn",TitleAr="الوصف انجليزي",TitleEn="Descriptor (English)", Lookup = false},
            new HeaderItem{Index=3,Key="OccupancyRate",TitleAr="نسب الاشغال",TitleEn="OccupancyRate", Lookup = false},
            new HeaderItem{Index=4,Key="OperatingRateInHoursPerDay",TitleAr="معدلات التشغيل",TitleEn="OperatingRateInHoursPerDay", Lookup = false},
            new HeaderItem{Index=5,Key="OperatingDaysPerMonth",TitleAr="أيام عمل الوحدة في الشهر",TitleEn="OperatingDaysPerMonth", Lookup = false},
            new HeaderItem{Index=6,Key="Category",TitleAr="الفئه الاساسيه",TitleEn="Category", Lookup = true},
            new HeaderItem{Index=7,Key="SubCategory",TitleAr="الفئه الفرعيه",TitleEn="Subcategory", Lookup = true},
            new HeaderItem{Index=8,Key="DataEffectiveDateFrom",TitleAr="البيان تاريخ التفعيل من",TitleEn="Data-Effective Date from", Lookup = false},
            new HeaderItem{Index=9,Key="DataEffectiveDateTo",TitleAr="البيان تاريخ التفعيل الي",TitleEn="Data-Effective Date to", Lookup = false},
        };

    }
}
