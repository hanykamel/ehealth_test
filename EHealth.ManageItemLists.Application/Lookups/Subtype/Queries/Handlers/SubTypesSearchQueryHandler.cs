using EHealth.ManageItemLists.Application.Lookups.SubCategories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.Queries;
using EHealth.ManageItemLists.Application.Lookups.Subtype.DTOs;
using EHealth.ManageItemLists.Domain.ItemListSubtypes;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Sub_Categories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.Subtype.Queries.Handlers
{
    public class SubTypesSearchQueryHandler : IRequestHandler<SubTypeSearchQuery, PagedResponse<SubTypeDto>>
    {
        private readonly IItemListSubtypeRepository _itemListSubtypeRepository;
        public SubTypesSearchQueryHandler(IItemListSubtypeRepository itemListSubtypeRepository)
        {
            _itemListSubtypeRepository = itemListSubtypeRepository;
        }
        public async Task<PagedResponse<SubTypeDto>> Handle(SubTypeSearchQuery request, CancellationToken cancellationToken)
        {
            var res = await ItemListSubtype.Search(_itemListSubtypeRepository, f => f.IsDeleted == false, request.PageNo, request.PageSize, request.EnablePagination);
            return new PagedResponse<SubTypeDto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = res.Data.Select(s => SubTypeDto.FromSubtype(s)).ToList()
            };
        }
    }
}
