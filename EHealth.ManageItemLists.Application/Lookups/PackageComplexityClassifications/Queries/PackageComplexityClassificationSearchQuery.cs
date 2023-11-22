using EHealth.ManageItemLists.Application.Lookups.PackageComplexityClassifications.DTOs;
using EHealth.ManageItemLists.Application.Lookups.PriceUnits.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.PackageComplexityClassifications.Queries
{
    public class PackageComplexityClassificationSearchQuery : LookupPagedRequerst, IRequest<PagedResponse<PackageComplexityClassificationDto>>
    {
    }
}
