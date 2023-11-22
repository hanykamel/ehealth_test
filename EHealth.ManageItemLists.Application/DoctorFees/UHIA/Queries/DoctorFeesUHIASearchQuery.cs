using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.DTOs;
using EHealth.ManageItemLists.Application.DoctorFees.UHIA.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.Queries
{
    public class DoctorFeesUHIASearchQuery : LookupPagedRequerst, IRequest<PagedResponse<DoctorFeesUHIADto>>
    {
        public int ItemListId { get; set; }
        public string? EHealthCode { get; set; }
        public string? DescriptorEn { get; set; }
        public string? DescriptorAr { get; set; }
        public string? ComplexityClassificationCode { get; set; }
        public string? OrderBy { get; set; }
        public bool? Ascending { get; set; }
    }
}
