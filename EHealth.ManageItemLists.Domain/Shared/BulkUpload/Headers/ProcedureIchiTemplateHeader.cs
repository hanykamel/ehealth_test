namespace EHealth.ManageItemLists.Domain.Shared.BulkUpload.Headers
{
    public class ProcedureIchiTemplateHeader
    {
        public static List<HeaderItem> Headers = new List<HeaderItem>
        {
           new HeaderItem{Index=0,Key="EHealthCode",TitleAr="كود أي هيلث",TitleEn="EHealthCode",Lookup=false},
           new HeaderItem{Index=1,Key="UHIAId",TitleAr="الكود الخاص بهيئه التأمين الصحي",TitleEn="UHIAId",Lookup = false},
           new HeaderItem{Index=2,Key="TitleAr",TitleAr="الوصف عربي",TitleEn="Title (Arabic)", Lookup = false},
           new HeaderItem{Index=3,Key="TitleEn",TitleAr="الوصف انجليزي",TitleEn="Title (English)", Lookup = false},

           new HeaderItem{Index=4,Key="Category",TitleAr="الفئه الاساسيه",TitleEn="Category", Lookup = true},
           new HeaderItem{Index=5,Key="SubCategory",TitleAr="الفئه الفرعيه",TitleEn="Subcategory", Lookup = true},
           new HeaderItem{Index=6,Key="LocalSpecialtyDepartment",TitleAr="التخصص المحلي",TitleEn="LocalSpecialtyDepartment", Lookup = true},

           new HeaderItem{Index=7,Key="DataEffectiveDateFrom",TitleAr="البيان تاريخ التفعيل من",TitleEn="Data-Effective Date from", Lookup = false},
           new HeaderItem{Index=8,Key="DataEffectiveDateTo",TitleAr="البيان تاريخ التفعيل الي",TitleEn="Data-Effective Date to", Lookup = false},

           new HeaderItem{Index=9,Key="Price",TitleAr="السعر",TitleEn="Price", Lookup = false},
           new HeaderItem{Index=10,Key="EffectiveDateFrom",TitleAr="السعر تاريخ التفعيل من",TitleEn="Price-Effective Date from", Lookup = false},
           new HeaderItem{Index=11,Key="EffectiveDateTo",TitleAr="السعر تاريخ التفعيل الي",TitleEn="Price-Effective Date to", Lookup = false},

        };
    }
}
