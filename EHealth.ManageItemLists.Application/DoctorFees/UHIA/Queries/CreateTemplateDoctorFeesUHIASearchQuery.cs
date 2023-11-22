using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using System.Data;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.Queries
{
    public class CreateTemplateDoctorFeesUHIASearchQuery : PagedRequerst, IRequest<DataTable>
    {
        public int ItemListId { get; set; }
        public string? Lang { get; set; }
        public string FormatType { get; set; }
    }
}
