using EHealth.ManageItemLists.Application.Resource.UHIA.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;

namespace EHealth.ManageItemLists.Application.Resource.UHIA.Queries
{
    public class ResourceUHIASearchQuery : LookupPagedRequerst, IRequest<PagedResponse<ResourceUHIADto>>
    {
        public int ItemListId { get; set; }
        public string? Code { get; set; }
        public string? DescriptorEn { get; set; }
        public string? DescriptorAr { get; set; }
        public string? CategoryEn { get; set; }
        public string? SubCategoryEn { get; set; }
        public string? OrderBy { get; set; }
        public bool? Ascending { get; set; }
    }
}
