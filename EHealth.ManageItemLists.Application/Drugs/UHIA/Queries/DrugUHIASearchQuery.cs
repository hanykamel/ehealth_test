using EHealth.ManageItemLists.Application.Drugs.UHIA.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.Queries
{
    public class DrugUHIASearchQuery : LookupPagedRequerst, IRequest<PagedResponse<DrugsUHIADto>>
    {
        public int ItemListId { get; set; }
        public string? EHealthCode { get;  set; }
        public string? LocalDrugCode { get;  set; }
        public string? InternationalNonProprietaryName { get;  set; }
        public string? ProprietaryName { get;  set; }
        public string? DosageForm { get;  set; }
        public string? RouteOfAdministration { get;  set; }
        public string? OrderBy { get; set; }
        public bool? Ascending { get; set; }
    }
}
