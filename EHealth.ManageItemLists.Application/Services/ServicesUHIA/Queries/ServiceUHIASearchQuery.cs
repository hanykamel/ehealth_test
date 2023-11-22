using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.Queries
{
    public class ServiceUHIASearchQuery : LookupPagedRequerst, IRequest<PagedResponse<ServiceUHIADto>>
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
