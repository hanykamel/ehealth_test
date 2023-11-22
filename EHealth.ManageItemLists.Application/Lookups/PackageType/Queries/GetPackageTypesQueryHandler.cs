using EHealth.ManageItemLists.Application.Lookups.PackageType.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;

namespace EHealth.ManageItemLists.Application.Lookups.PackageType.Queries
{
    public class GetPackageTypesQueryHandler : IRequestHandler<GetPackageTypesQuery, PagedResponse<PackageTypeDto>>
    {
        private readonly IPackageTypeRepository _packageTypeRepository;

        public GetPackageTypesQueryHandler(IPackageTypeRepository packageTypeRepository)
        {
            _packageTypeRepository = packageTypeRepository;
        }
        public async Task<PagedResponse<PackageTypeDto>> Handle(GetPackageTypesQuery request, CancellationToken cancellationToken)
        {
            var res = await EHealth.ManageItemLists.Domain.PackageTypes.PackageType.Search(_packageTypeRepository, f => f.IsDeleted == false, request.PageNo, request.PageSize, request.EnablePagination);
            return new PagedResponse<PackageTypeDto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = res.Data.Select(s => PackageTypeDto.FromPackageType(s)).ToList()
            };
        }
    }
}
