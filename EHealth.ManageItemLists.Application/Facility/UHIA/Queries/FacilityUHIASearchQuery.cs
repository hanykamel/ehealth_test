using EHealth.ManageItemLists.Application.Facility.UHIA.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Facility.UHIA.Queries
{
    public class FacilityUHIASearchQuery : LookupPagedRequerst, IRequest<PagedResponse<FacilityUHIADto>>
    {
        public int ItemListId { get; set; }
        public string? Code { get; set; }
        public string? DescriptorAr { get; set; }
        public string? DescriptorEn { get; set; }
        public string? OrderBy { get; set; }
        public bool? Ascending { get; set; }

    }
}
