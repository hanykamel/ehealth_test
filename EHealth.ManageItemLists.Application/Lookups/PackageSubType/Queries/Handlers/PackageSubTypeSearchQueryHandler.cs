using EHealth.ManageItemLists.Application.Lookups.PackageComplexityClassifications.DTOs;
using EHealth.ManageItemLists.Application.Lookups.PackageComplexityClassifications.Queries;
using EHealth.ManageItemLists.Application.Lookups.PackageSubType.DTOS;
using EHealth.ManageItemLists.Domain.PackageComplexityClassifications;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EHealth.ManageItemLists.Application.Lookups.PackageSubType.Queries.Handlers
{
    using PackageSubType = EHealth.ManageItemLists.Domain.PackageSubTypes.PackageSubType;
    public class PackageSubTypeSearchQueryHandler : IRequestHandler<PackageSubTypeSearchQuery, PagedResponse<PackageSubTypeDTO>>
    {
        private readonly IPackageSubTypeRepository _PackageSubTypeRepository;
        public PackageSubTypeSearchQueryHandler(IPackageSubTypeRepository PackageSubTypeRepository)
        {
            _PackageSubTypeRepository = PackageSubTypeRepository;
        }


        public async Task<PagedResponse<PackageSubTypeDTO>> Handle(PackageSubTypeSearchQuery request, CancellationToken cancellationToken)
        {
            var res = await PackageSubType.Search(_PackageSubTypeRepository, f => f.IsDeleted == false, request.PageNo, request.PageSize, request.EnablePagination);
            return new PagedResponse<PackageSubTypeDTO>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = res.Data.Select(s => PackageSubTypeDTO.FromPackageSubType(s)).ToList()
            };
        }
    }
}
