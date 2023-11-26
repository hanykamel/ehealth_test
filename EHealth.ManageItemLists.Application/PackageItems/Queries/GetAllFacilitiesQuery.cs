using EHealth.ManageItemLists.Application.Facility.UHIA.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace EHealth.ManageItemLists.Application.PackageItems.Queries
{
    public class GetAllFacilitiesQuery :LookupPagedRequerst, IRequest<PagedResponse<GetAllFacilitiesDTO>>
    {
        [Required]
        public Guid PackageHeaderId { get;  set; }
        public string? Code { get; set; }
        public string? DescriptorAr { get; set; }
        public string? DescriptorEn { get; set; }
        public string? OrderBy { get; set; }
        public bool? Ascending { get; set; }

    }
}
