using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Categories.Queries;
using EHealth.ManageItemLists.Application.Lookups.PriceUnits.DTOs;
using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.PriceUnits;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.PriceUnits.Queries.Handler
{
    public class PriceUnitSearchQueryHandler : IRequestHandler<PriceUnitSearchQuery, PagedResponse<PriceUnitDto>>
    {
        private readonly IPriceUnitRepository _priceUnitRepository;
        public PriceUnitSearchQueryHandler(IPriceUnitRepository priceUnitRepository)
        {
            _priceUnitRepository = priceUnitRepository;
        }
        public async Task<PagedResponse<PriceUnitDto>> Handle(PriceUnitSearchQuery request, CancellationToken cancellationToken)
        {
            var res = await PriceUnit.Search(_priceUnitRepository, f => f.IsDeleted == false, request.PageNo, request.PageSize, request.EnablePagination);
            return new PagedResponse<PriceUnitDto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = res.Data.Select(s => PriceUnitDto.FromPriceUnit(s)).ToList()
            };
        }
    }
}
