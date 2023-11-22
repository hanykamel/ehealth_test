namespace EHealth.ManageItemLists.Domain.Shared.BulkUpload.Headers
{
    public class DrugsUhiaTemplateHeader
    {
        public static List<HeaderItem> Headers = new List<HeaderItem>
        {
              new HeaderItem{Index=0,Key="EHealthDrugCode",TitleAr="كود أي هيلث",TitleEn="EHealthDrugCode",Lookup=false},
              new HeaderItem{Index=1,Key="LocalDrugCode",TitleAr="كود الدواء المحلي",TitleEn="LocalDrugCode",Lookup=false},
              new HeaderItem{Index=2,Key="InternationalNonProprietaryName",TitleAr="المواد الفعالة",TitleEn="InternationalNonProprietaryName",Lookup=false},
              new HeaderItem{Index=3,Key="ProprietaryName",TitleAr="الاسم التجاري",TitleEn="ProprietaryName",Lookup=false},
              new HeaderItem{Index=4,Key="DosageForm",TitleAr="الشكل الدوائي",TitleEn="DosageForm",Lookup=false},
              new HeaderItem{Index=5,Key="RouteOfAdministration",TitleAr="طريقة الاعطاء",TitleEn="RouteOfAdministration",Lookup=false},
              new HeaderItem{Index=6,Key="Manufacturer",TitleAr="الشركة المصنعه",TitleEn="Manufacturer",Lookup=false},
              new HeaderItem{Index=7,Key="MarketAuthorizationHolder",TitleAr="صاحب ترخيص السوق",TitleEn="MarketAuthorizationHolder",Lookup=false},

              new HeaderItem{Index=8,Key="RegistrationType",TitleAr="نوع التسجيل",TitleEn="RegistrationType", Lookup = true},
              new HeaderItem{Index=9,Key="DrugsPackageType",TitleAr="نوع العبوة",TitleEn="DrugsPackageType", Lookup = true},
              new HeaderItem{Index=10,Key="MainUnit",TitleAr="الوحدة الاساسيه",TitleEn="MainUnit", Lookup = true},
              new HeaderItem{Index=11,Key="NumberOfMainUnit",TitleAr="عدد الوحدة الاساسيه",TitleEn="NumberOfMainUnit",Lookup=false},
              new HeaderItem{Index=12,Key="SubUnit",TitleAr="الوحدة الفرعيه",TitleEn="SubUnit", Lookup = true},
              new HeaderItem{Index=13,Key="NumberOfSubunitPerMainUnit",TitleAr="عدد الوحدة الصغري",TitleEn="NumberOfSubunitPerMainUnit",Lookup=false},
              new HeaderItem{Index=14,Key="TotalNumberSubunitsOfPack",TitleAr="اجمالي عدد الوحدة الصغري",TitleEn="TotalNumberSubunitsOfPack",Lookup=false},
              new HeaderItem{Index=15,Key="ReimbursementCategory",TitleAr="فئة التعويضات ",TitleEn="ReimbursementCategory", Lookup = true},

              new HeaderItem{Index=16,Key="DataEffectiveDateFrom",TitleAr="البيان تاريخ التفعيل من",TitleEn="Data-Effective Date from", Lookup = false},
              new HeaderItem{Index=17,Key="DataEffectiveDateTo",TitleAr="البيان تاريخ التفعيل الي",TitleEn="Data-Effective Date to", Lookup = false},
              new HeaderItem{Index=18,Key="MainUnitPrice",TitleAr="سعر مشاركة التامين للوحدة الأساسية",TitleEn="MainUnitPrice", Lookup = false},
              new HeaderItem{Index=19,Key="FullPackPrice",TitleAr="سعر مشاركة التامين للعبوة الكاملة",TitleEn="FullPackPrice", Lookup = false},
              new HeaderItem{Index=20,Key="SubUnitPrice",TitleAr="سعر مشاركة التامين للوحدة الصغرى",TitleEn="SubUnitPrice", Lookup = false},
      
              new HeaderItem{Index=21,Key="EffectiveDateFrom",TitleAr="السعر تاريخ التفعيل من",TitleEn="Price-Effective Date from", Lookup = false},
              new HeaderItem{Index=22,Key="EffectiveDateTo",TitleAr="السعر تاريخ التفعيل الي",TitleEn="Price-Effective Date to", Lookup = false},
        };
    }
}
