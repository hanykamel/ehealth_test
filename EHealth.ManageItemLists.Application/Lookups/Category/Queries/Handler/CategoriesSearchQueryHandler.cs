using EHealth.ManageItemLists.Application.Facility.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Facility.UHIA.Queries;
using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.Categories.Queries.Handler
{
    public class CategoriesSearchQueryHandler : IRequestHandler<CategoriesSearchQuery, PagedResponse<CategoryDto>>
    {
        private readonly ICategoriesRepository _categoriesRepository;
        public CategoriesSearchQueryHandler(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }
        public async Task<PagedResponse<CategoryDto>> Handle(CategoriesSearchQuery request, CancellationToken cancellationToken)
        {
            var res = await Category.Search(_categoriesRepository, f => f.IsDeleted == false && f.ItemListSubtypeId == request.ItemListSubtypeId, request.PageNo, request.PageSize, request.EnablePagination);
            return new PagedResponse<CategoryDto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = res.Data.Select(s => CategoryDto.FromCategory(s)).ToList()
            };
        }
    }
}
