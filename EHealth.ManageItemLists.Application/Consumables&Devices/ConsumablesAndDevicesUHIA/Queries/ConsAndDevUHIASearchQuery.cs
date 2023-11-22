using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Queries
{
    public class ConsAndDevUHIASearchQuery : LookupPagedRequerst, IRequest<PagedResponse<ConsAndDevDto>>
    {
        public int ItemListId { get; set; }
        public string? EHealthCode { get; set; }
        public string? UHIAId { get; set; }
        public string? ShortDescriptionAr { get; set; }
        public string? ShortDescriptionEn { get; set; }
        public string? OrderBy { get; set; }
        public bool? Ascending { get; set; }
    }
}
