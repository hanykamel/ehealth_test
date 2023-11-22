using EHealth.ManageItemLists.Application.PackageHeaders.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.PackageHeaders.Queries
{
    public class PackageHeaderSearchQuery : LookupPagedRequerst, IRequest<PagedResponse<PackageHeaderDTO>>
    {
        public string? EHealthCode { get; set; }
        public string? UHIACode { get; set; }
        public string? NameEn { get; set; }
        public string? NameAr { get;  set; }
        public string? OrderBy { get; set; }
        public bool? Ascending { get; set; }
    }
}
