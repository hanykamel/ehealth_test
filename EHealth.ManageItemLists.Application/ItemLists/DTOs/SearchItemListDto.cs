using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.Shared.Pagination;

namespace EHealth.ManageItemLists.Application.ItemLists.DTOs
{
    public class SearchItemListDto:PagedRequerst
    {
        public int? Id { get; private set; }
        public string? Code { get; private set; }
        public string? NameAr { get; private set; }
        public string? NameEN { get; private set; }
        public int? ItemListSubtypeId { get; private set; }
        //public bool NeedSubmission { get; private set; }
        //public bool NeedReview { get; private set; }
       
      
      
    }
}
