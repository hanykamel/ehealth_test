using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Categories.Queries;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.DTOs;
using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Sub_Categories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.SubCategories.Queries.Handlers
{
    public class SubCategoriesSearchQueryHandler : IRequestHandler<SubCategoriesSearchQuery, PagedResponse<SubCategoryDto>>
    {
        private readonly ISubCategoriesRepository _subCategoriesRepository;
        public SubCategoriesSearchQueryHandler(ISubCategoriesRepository subCategoriesRepository)
        {
            _subCategoriesRepository = subCategoriesRepository;
        }
        public async Task<PagedResponse<SubCategoryDto>> Handle(SubCategoriesSearchQuery request, CancellationToken cancellationToken)
        {
            var res = await SubCategory.Search(_subCategoriesRepository, f => f.IsDeleted == false && f.ItemListSubtypeId == request.ItemListSubtypeId, request.PageNo, request.PageSize, request.EnablePagination);
            return new PagedResponse<SubCategoryDto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = res.Data.Select(s => SubCategoryDto.FromSubCategory(s)).ToList()
            };
        }
    }
}
