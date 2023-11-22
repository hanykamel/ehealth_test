namespace EHealth.ManageItemLists.Domain.Shared.BulkUpload.Headers
{
    public class ItemListTemplateHeader
    {
        public static List<HeaderItem> Headers = new List<HeaderItem>
        {  
            new HeaderItem{Index=0,Key="Code",TitleAr="رمز القائمة",TitleEn="Code",Lookup=false},
            new HeaderItem{Index=1,Key="NameAr",TitleAr="الوصف عربي",TitleEn="NameAr",Lookup=false},
            new HeaderItem{Index=2,Key="NameEN",TitleAr="الوصف انجليزي",TitleEn="NameEN",Lookup = false},
            new HeaderItem{Index=3,Key="ItemType",TitleAr="الوصف انجليزي",TitleEn="ItemType",Lookup = true},
            new HeaderItem{Index=4,Key="ItemListSubtype",TitleAr="الوصف عربي",TitleEn="ItemListSubtype", Lookup = true},
        };
    }
}
