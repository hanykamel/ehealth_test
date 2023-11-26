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
    public class GetAllDoctorsFeesQuery : LookupPagedRequerst, IRequest<PagedResponse<GetAllDoctorsFeesDTO>>
    {
        [Required]
        public Guid PackageHeaderId { get; set; }
        public string? Code { get; set; }
        public string? ItemAr { get; set; }
        public string? ItemEn { get; set; }
        public int? ComplexityClassificationCode { get; set; }
        public string? OrderBy { get; set; }
        public bool? Ascending { get; set; }
    }
}
