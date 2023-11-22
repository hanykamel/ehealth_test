using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Categories.Queries;
using EHealth.ManageItemLists.Application.Lookups.LocalUnitOfMeasures.DTOs;
using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.LocalUnitOfMeasures.Queries.Handler
{
    public class LocalUnitOfMeasuresSearchQueryHandler : IRequestHandler<LocalUnitOfMeasuresSearchQuery, PagedResponse<UnitOfMeasureDto>>
    {
        private readonly IUnitOfMeasureRepository _unitOfMeasureRepository;
        public LocalUnitOfMeasuresSearchQueryHandler(IUnitOfMeasureRepository unitOfMeasureRepository)
        {
            _unitOfMeasureRepository = unitOfMeasureRepository;
        }
        public async Task<PagedResponse<UnitOfMeasureDto>> Handle(LocalUnitOfMeasuresSearchQuery request, CancellationToken cancellationToken)
        {
            var res = await UnitOfMeasure.Search(_unitOfMeasureRepository, f => f.IsDeleted == false, request.PageNo, request.PageSize, request.EnablePagination);
            return new PagedResponse<UnitOfMeasureDto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = res.Data.Select(s => UnitOfMeasureDto.FromLocalUnitOfMeasure(s)).ToList()
            };
        }
    }
}
