using EHealth.ManageItemLists.Application.Lookups.LocalSpecialtyDepartment.DTOs;
using EHealth.ManageItemLists.Application.Lookups.LocalSpecialtyDepartment.Queries;
using EHealth.ManageItemLists.Application.Lookups.UnitsTypes.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.UnitsTypes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.UnitsTypes.Queries.Handler
{
    public class UnitsTypesSearchQueryHandler : IRequestHandler<UnitsTypesSearchQuery, PagedResponse<UnitsTypeDto>>
    {
        private readonly IUnitsTypeRepository _unitsTypeRepository;
        public UnitsTypesSearchQueryHandler(IUnitsTypeRepository unitsTypeRepository)
        {
            _unitsTypeRepository = unitsTypeRepository;
        }
        public async Task<PagedResponse<UnitsTypeDto>> Handle(UnitsTypesSearchQuery request, CancellationToken cancellationToken)
        {
            var res = await UnitsType.Search(_unitsTypeRepository, f => f.IsDeleted == false, request.PageNo, request.PageSize, request.EnablePagination);
            return new PagedResponse<UnitsTypeDto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = res.Data.Select(s => UnitsTypeDto.FromUnitsType(s)).ToList()
            };
        }
    }
}
