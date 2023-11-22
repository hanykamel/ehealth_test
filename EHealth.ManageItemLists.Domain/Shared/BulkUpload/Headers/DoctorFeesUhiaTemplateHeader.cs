namespace EHealth.ManageItemLists.Domain.Shared.BulkUpload.Headers
{
    public class DoctorFeesUhiaTemplateHeader
    {
        public static List<HeaderItem> Headers = new List<HeaderItem>
        {
            new HeaderItem{Index=0,Key="EHealthCode",TitleAr="كود أي هيلث",TitleEn="EHealthCode",Lookup=false},
            new HeaderItem{Index=1,Key="DescriptorAr",TitleAr="الوصف عربي",TitleEn="DescriptorAr", Lookup = false},
            new HeaderItem{Index=2,Key="DescriptorEn",TitleAr="الوصف انجليزي",TitleEn="DescriptorEn", Lookup = false},
            new HeaderItem{Index=3,Key="PackageComplexityClassification",TitleAr="توصيف العمليه",TitleEn="PackageComplexityClassification", Lookup = true},
            new HeaderItem{Index=4,Key="DataEffectiveDateFrom",TitleAr="البيان تاريخ التفعيل من",TitleEn="Data-Effective Date from", Lookup = false},
            new HeaderItem{Index=5,Key="DataEffectiveDateTo",TitleAr="البيان تاريخ التفعيل الي",TitleEn="Data-Effective Date to", Lookup = false},
            new HeaderItem{Index=6,Key="DoctorFees",TitleAr="السعر",TitleEn="DoctorFees", Lookup = false},
            new HeaderItem{Index=7,Key="UnitOfDoctorFees",TitleAr="وحدة اتعاب",TitleEn="UnitOfDoctorFees", Lookup = true},
            new HeaderItem{Index=8,Key="EffectiveDateFrom",TitleAr="السعر تاريخ التفعيل من",TitleEn="Price-Effective Date from", Lookup = false},
            new HeaderItem{Index=9,Key="EffectiveDateTo",TitleAr="السعر تاريخ التفعيل الي",TitleEn="Price-Effective Date to", Lookup = false},
        };

    }
}
