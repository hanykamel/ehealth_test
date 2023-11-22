using EHealth.ManageItemLists.Application.Lookups.LocalUnitOfMeasures.DTOs;
using EHealth.ManageItemLists.Application.Lookups.LocalUnitOfMeasures.Queries;
using EHealth.ManageItemLists.Application.Lookups.ReimbursementCategories.DTOs;
using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
using EHealth.ManageItemLists.Domain.ReimbursementCategories;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.ReimbursementCategories.Queries.Handlers
{
    public class ReimbursementCategoriesSearchQueryHandler : IRequestHandler<ReimbursementCategoriesSearchQuery, PagedResponse<ReimbursementCategoryDto>>
    {
        private readonly IReimbursementCategoryRepository _reimbursementCategoryRepository;
        public ReimbursementCategoriesSearchQueryHandler(IReimbursementCategoryRepository reimbursementCategoryRepository)
        {
            _reimbursementCategoryRepository = reimbursementCategoryRepository;
        }
        public async Task<PagedResponse<ReimbursementCategoryDto>> Handle(ReimbursementCategoriesSearchQuery request, CancellationToken cancellationToken)
        {
            var res = await ReimbursementCategory.Search(_reimbursementCategoryRepository, f => f.IsDeleted == false, request.PageNo, request.PageSize, request.EnablePagination);
            return new PagedResponse<ReimbursementCategoryDto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = res.Data.Select(s => ReimbursementCategoryDto.FromReimbursementCategory(s)).ToList()
            };
        }
    }
}
