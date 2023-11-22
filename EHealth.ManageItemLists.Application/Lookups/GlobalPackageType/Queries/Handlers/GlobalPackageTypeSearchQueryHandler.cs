using EHealth.ManageItemLists.Application.Lookups.GlobalPackageType.DTOs;


using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Infrastructure.Repositories.Lookups;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.GlobalPackageType.Queries.Handlers
{
    using GlobalPackageType = EHealth.ManageItemLists.Domain.GlobelPackageTypes.GlobelPackageType;
    public class GlobalPackageTypeSearchQueryHandler : IRequestHandler<GlobalPackageTypeSearchQuery, PagedResponse<GlobalPackageTypeDTO>>
    {
        private readonly IGlobelPackageTypeRepository _globelPackageTypeRepository;

        public GlobalPackageTypeSearchQueryHandler(IGlobelPackageTypeRepository globelPackageTypeRepository)
        {
            _globelPackageTypeRepository = globelPackageTypeRepository;
        }

        public async Task<PagedResponse<GlobalPackageTypeDTO>> Handle(GlobalPackageTypeSearchQuery request, CancellationToken cancellationToken)
        {
            var result = await GlobalPackageType.Search(_globelPackageTypeRepository, g => g.IsDeleted == false, request.PageNo, request.PageSize, request.EnablePagination);
            return new PagedResponse<GlobalPackageTypeDTO>
            {
                PageNumber = result.PageNumber,
                TotalCount = result.TotalCount,
                PageSize = result.PageSize,
                Data = result.Data.Select(p => GlobalPackageTypeDTO.FromGlobalPackageType(p)).ToList()
            };
        }
    }
}
