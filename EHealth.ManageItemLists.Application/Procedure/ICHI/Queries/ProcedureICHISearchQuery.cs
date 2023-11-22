using EHealth.ManageItemLists.Application.Procedure.ICHI.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;

namespace EHealth.ManageItemLists.Application.Procedure.ICHI.Queries
{
    public class ProcedureICHISearchQuery : LookupPagedRequerst, IRequest<PagedResponse<ProcedureICHIDto>>
    {
        public int ItemListId { get; set; }
        public string? EHealthCode { get; set; }
        public string? UHIAId { get; set; }
        public string? TitleAr { get; set; }
        public string? TitleEn { get; set; }
        public string? OrderBy { get; set; }
        public bool? Ascending { get; set; }
    }
}
