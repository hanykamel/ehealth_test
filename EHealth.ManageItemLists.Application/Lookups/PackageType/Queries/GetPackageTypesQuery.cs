using EHealth.ManageItemLists.Application.Lookups.PackageType.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;

namespace EHealth.ManageItemLists.Application.Lookups.PackageType.Queries
{
    public class GetPackageTypesQuery : LookupPagedRequerst, IRequest<PagedResponse<PackageTypeDto>>
    {
        //add properties here using PackageType class properties
      
    }
}
