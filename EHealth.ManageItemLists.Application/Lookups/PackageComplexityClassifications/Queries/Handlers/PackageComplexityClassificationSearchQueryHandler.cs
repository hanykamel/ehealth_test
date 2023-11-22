using EHealth.ManageItemLists.Application.Lookups.PackageComplexityClassifications.DTOs;
using EHealth.ManageItemLists.Application.Lookups.PriceUnits.DTOs;
using EHealth.ManageItemLists.Application.Lookups.PriceUnits.Queries;
using EHealth.ManageItemLists.Domain.PackageComplexityClassifications;
using EHealth.ManageItemLists.Domain.PriceUnits;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.PackageComplexityClassifications.Queries.Handlers
{
    public class PackageComplexityClassificationSearchQueryHandler : IRequestHandler<PackageComplexityClassificationSearchQuery, PagedResponse<PackageComplexityClassificationDto>>
    {
        private readonly IPackageComplexityClassificationRepository _packageComplexityClassificationRepository;
        public PackageComplexityClassificationSearchQueryHandler(IPackageComplexityClassificationRepository packageComplexityClassificationRepository)
        {
            _packageComplexityClassificationRepository = packageComplexityClassificationRepository;
        }
        public async Task<PagedResponse<PackageComplexityClassificationDto>> Handle(PackageComplexityClassificationSearchQuery request, CancellationToken cancellationToken)
        {
            var res = await PackageComplexityClassification.Search(_packageComplexityClassificationRepository, f => f.IsDeleted == false, request.PageNo, request.PageSize, request.EnablePagination);
            return new PagedResponse<PackageComplexityClassificationDto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = res.Data.Select(s => PackageComplexityClassificationDto.FromPackageComplexityClassification(s)).ToList()
            };
        }
    }
}
