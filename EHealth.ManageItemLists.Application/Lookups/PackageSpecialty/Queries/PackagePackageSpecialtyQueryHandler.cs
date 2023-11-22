using EHealth.ManageItemLists.Application.Lookups.PackageComplexityClassifications.DTOs;
using EHealth.ManageItemLists.Application.Lookups.PackageSpecialty.DTOs;
using EHealth.ManageItemLists.Domain.PackageComplexityClassifications;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;

namespace EHealth.ManageItemLists.Application.Lookups.PackageSpecialty.Queries
{
    using PackageSpecialty = EHealth.ManageItemLists.Domain.PackageSpecialties.PackageSpecialty;
    public class PackagePackageSpecialtyQueryHandler : IRequestHandler<PackagePackageSpecialtyQuery, PagedResponse<PackageSpecialtyDto>>
    {
        private readonly IPackageSpecialtiesRepository _packageSpecialtiesRepository;
        public PackagePackageSpecialtyQueryHandler(IPackageSpecialtiesRepository packageSpecialtiesRepository)
        {
            _packageSpecialtiesRepository = packageSpecialtiesRepository;
        }
        public async Task<PagedResponse<PackageSpecialtyDto>> Handle(PackagePackageSpecialtyQuery request, CancellationToken cancellationToken)
        {
            var res = await PackageSpecialty.Search(_packageSpecialtiesRepository, f => f.IsDeleted == false, request.PageNo, request.PageSize, request.EnablePagination);
            return new PagedResponse<PackageSpecialtyDto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = res.Data.Select(s => PackageSpecialtyDto.FromPackageSpecialty(s)).ToList()
            };
        }
    }
}
