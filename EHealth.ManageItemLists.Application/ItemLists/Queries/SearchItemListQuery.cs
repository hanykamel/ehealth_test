using EHealth.ManageItemLists.Application.ItemLists.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;

namespace EHealth.ManageItemLists.Application.ItemLists.Queries
{

    //public record SearchItemListQuery(SearchItemListDto searchItemListDto) : IRequest<PagedResponse<ItemListDto>>;
    public class SearchItemListQuery :LookupPagedRequerst ,IRequest<PagedResponse<ItemListDto>>
    {
        public int? Id { get;  set; }
        public string? Code { get;  set; }
        public string? NameAr { get;  set; }
        public string? NameEN { get;  set; }
        public int? ItemListSubtypeId { get;  set; }
        public int? ItemListTypeId { get; set; }
        public string? OrderBy { get; set; }
        public bool? Ascending { get; set; }
    }
}
