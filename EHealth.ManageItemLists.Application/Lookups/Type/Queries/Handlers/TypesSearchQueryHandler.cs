using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Categories.Queries;
using EHealth.ManageItemLists.Application.Lookups.Type.DTOs;
using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.ItemListTypes;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Infrastructure.Repositories.Lookups;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.Type.Queries.Handlers
{
    public class TypesSearchQueryHandler : IRequestHandler<TypesSearchQuery, PagedResponse<TypeDto>>
    {
        private readonly IItemListTypeRepository _typesRepository;
        public TypesSearchQueryHandler(IItemListTypeRepository typesRepository)
        {
            _typesRepository = typesRepository;
        }
        public async Task<PagedResponse<TypeDto>> Handle(TypesSearchQuery request, CancellationToken cancellationToken)
        {
            var res = await ItemListType.Search(_typesRepository, f => f.IsDeleted == false, request.PageNo, request.PageSize, request.EnablePagination);
            return new PagedResponse<TypeDto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = res.Data.Select(s => TypeDto.FromType(s)).ToList()
            };
        }
    }
}
