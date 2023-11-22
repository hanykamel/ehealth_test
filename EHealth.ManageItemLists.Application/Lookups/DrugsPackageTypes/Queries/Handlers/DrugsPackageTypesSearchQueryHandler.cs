using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Categories.Queries;
using EHealth.ManageItemLists.Application.Lookups.DrugsPackageTypes.DTOs;
using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.DrugsPackageTypes;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.DrugsPackageTypes.Queries.Handlers
{

    public class DrugsPackageTypesSearchQueryHandler : IRequestHandler<DrugsPackageTypesSearchQuery, PagedResponse<DrugsPackageTypesDto>>
    {
        private readonly IDrugsPackageTypeRepository _drugsPackageTypeRepository;
        public DrugsPackageTypesSearchQueryHandler(IDrugsPackageTypeRepository drugsPackageTypeRepository)
        {
            _drugsPackageTypeRepository = drugsPackageTypeRepository;
        }
        public async Task<PagedResponse<DrugsPackageTypesDto>> Handle(DrugsPackageTypesSearchQuery request, CancellationToken cancellationToken)
        {
            var res = await DrugsPackageType.Search(_drugsPackageTypeRepository, f => f.IsDeleted == false, request.PageNo, request.PageSize, request.EnablePagination);
            return new PagedResponse<DrugsPackageTypesDto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = res.Data.Select(s => DrugsPackageTypesDto.FromDrugsPackageType(s)).ToList()
            };
        }
    }
}
