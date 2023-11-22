using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using System.Data;

namespace EHealth.ManageItemLists.Application.Procedure.ICHI.Queries
{
    public class CreateTemplateProcedureICHISearchQuery : PagedRequerst, IRequest<DataTable>
    {
        public int ItemListId { get; set; }
        public string? Lang { get; set; }
        
        public string FormatType { get; set; }
    }
}
