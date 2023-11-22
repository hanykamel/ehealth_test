using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.DTOs;
using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Queries;
using EHealth.ManageItemLists.Application.Procedure.ICHI.DTOs;
using EHealth.ManageItemLists.Application.Resource.UHIA.DTOs;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Resource.UHIA.Queries.Handler
{
    public class ResourceUHIASearchQueryHandler : IRequestHandler<ResourceUHIASearchQuery, PagedResponse<ResourceUHIADto>>
    {
        private readonly IResourceUHIARepository _resourceUHIARepository;
        public ResourceUHIASearchQueryHandler(IResourceUHIARepository resourceUHIARepository)
        {
            _resourceUHIARepository = resourceUHIARepository;
        }
        public async Task<PagedResponse<ResourceUHIADto>> Handle(ResourceUHIASearchQuery request, CancellationToken cancellationToken)
        {
            var res = await ResourceUHIA.Search(_resourceUHIARepository, f =>
            f.ItemListId == request.ItemListId &&
            (!string.IsNullOrEmpty(request.Code) ? f.Code.ToLower().Contains(request.Code.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.DescriptorEn) ? f.DescriptorEn.ToLower().Contains(request.DescriptorEn.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.DescriptorAr) ? f.DescriptorAr.ToLower().Contains(request.DescriptorAr.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.CategoryEn) && f.Category != null ? f.Category.CategoryEn.ToLower().Contains(request.CategoryEn.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.SubCategoryEn) && f.SubCategory != null ? f.SubCategory.SubCategoryEn.ToLower().Contains(request.SubCategoryEn.ToLower()) : true)
            //
            //&& f.IsDeleted != true
            , request.PageNo, request.PageSize,request.EnablePagination, request.OrderBy, request.Ascending);

            var data = res.Data.Select(s => ResourceUHIADto.FromResourceUHIA(s)).ToList();

            return new PagedResponse<ResourceUHIADto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = data
            };
        }
    }
}
