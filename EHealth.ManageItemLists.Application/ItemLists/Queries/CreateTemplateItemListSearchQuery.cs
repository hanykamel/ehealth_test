using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using System.Data;

namespace EHealth.ManageItemLists.Application.ItemLists.Queries
{
    public class CreateTemplateItemListSearchQuery : PagedRequerst, IRequest<DataTable>
    {
        public string? Lang { get; set; }
        public string FormatType { get; set; }
    }
}
