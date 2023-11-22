using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.BulkUpload.Headers
{
    public class ServiceUhiaTemplateHeader
    {
        public static List<HeaderItem> Headers = new List<HeaderItem>
        {
            new HeaderItem{Index=0,Key="EHealthCode",TitleAr="كود أي هيلث",TitleEn="EHealthCode",Lookup=false},
            new HeaderItem{Index=1,Key="UHIAId",TitleAr="الكود الخاص بهيئه التأمين الصحي",TitleEn="UHIAId",Lookup = false},
            new HeaderItem{Index=2,Key="ShortDescAr",TitleAr="الوصف عربي",TitleEn="Short Description (Arabic)", Lookup = false},
            new HeaderItem{Index =3, Key = "ShortDescEn", TitleAr = "الوصف انجليزي", TitleEn = "Short Description (English)", Lookup = false},
            new HeaderItem{Index=4,Key="Category",TitleAr="الفئه الاساسيه",TitleEn="ServiceCategory", Lookup = true},
            new HeaderItem{Index=5,Key="SubCategory",TitleAr="الفئه الفرعيه",TitleEn="Subcategory", Lookup = true},
            new HeaderItem{Index=6,Key="DataEffectiveDateFrom",TitleAr="البيان تاريخ التفعيل من",TitleEn="Data-Effective Date from", Lookup = false},
            new HeaderItem{Index=7,Key="DataEffectiveDateTo",TitleAr="البيان تاريخ التفعيل الي",TitleEn="Data-Effective Date to", Lookup = false},
            new HeaderItem{Index=8,Key="Price",TitleAr="السعر",TitleEn="Price", Lookup = false},
            new HeaderItem{Index=9,Key="EffectiveDateFrom",TitleAr="السعر تاريخ التفعيل من",TitleEn="Price-Effective Date from", Lookup = false},
            new HeaderItem{Index=10,Key="EffectiveDateTo",TitleAr="السعر تاريخ التفعيل الي",TitleEn="Price-Effective Date to", Lookup = false},


        };
    }
}

