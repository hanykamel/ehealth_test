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
    public class GetAllDrugsQuery : LookupPagedRequerst, IRequest<PagedResponse<GetAllDrugsDTO>>
    {
        [Required]
        public Guid PackageHeaderId { get; set; }
        public string? ProprietaryName { get; set; }
        public string? EHealthDrugCode { get; set; }
        public string? LocalDrugCode { get; set; }
        public string? OrderBy { get; set; }
        public bool? Ascending { get; set; }

    }
}
