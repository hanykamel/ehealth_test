using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using System.Data;

namespace EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Queries
{
    public class CreateTemplateDevicesAndAssetUHIASearchQuery : PagedRequerst, IRequest<DataTable>
    {
        public int ItemListId { get; set; }
        public string? Lang { get; set; }
        public string FormatType { get; set; }
    }
}
