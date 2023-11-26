using EHealth.ManageItemLists.Application.PackageItems.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.PackageItems.Queries
{
    public class GetAllConsumablesAndDevicesQuery : LookupPagedRequerst, IRequest<PagedResponse<GetAllConsumablesAndDevicesDTO>>
    {
        [Required]
        public Guid PackageHeaderId { get; set; }
        public string? eHealthCode { get; set; }
        public string? UHIAId { get; set; }
        public string? ItemNameAr { get; set; }
        public string? ItemNameEn { get; set; }
        public string? OrderBy { get; set; }
        public bool? Ascending { get; set; }
    }
}
