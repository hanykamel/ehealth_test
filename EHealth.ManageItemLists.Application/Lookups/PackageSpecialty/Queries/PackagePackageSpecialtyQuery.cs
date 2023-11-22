using EHealth.ManageItemLists.Application.Lookups.PackageSpecialty.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.PackageSpecialty.Queries
{
    public class PackagePackageSpecialtyQuery :LookupPagedRequerst, IRequest<PagedResponse<PackageSpecialtyDto>>
    {
    }
}
