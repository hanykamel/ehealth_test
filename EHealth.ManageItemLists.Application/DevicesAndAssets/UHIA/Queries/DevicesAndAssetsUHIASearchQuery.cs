using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;

namespace EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Queries
{
    public class DevicesAndAssetsUHIASearchQuery : LookupPagedRequerst, IRequest<PagedResponse<DevicesAndAssetsUHIADto>>
    {
        public int ItemListId { get; set; }
        public string? Code { get; set; }
        public string? DescriptorEn { get; set; }
        public string? DescriptorAr { get; set; }
        public string? UnitRoomEn { get; set; }
        public string? CategoryEn { get; set; }
        public string? SubCategoryEn { get; set; }
        public string? OrderBy { get; set; }
        public bool? Ascending { get; set; }
    }
}
