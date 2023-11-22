using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs;
using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Facility.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Facility.UHIA.Queries;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Queries.Handler
{
    public class DevicesAndAssetsUHIASearchQueryHandler : IRequestHandler<DevicesAndAssetsUHIASearchQuery, PagedResponse<DevicesAndAssetsUHIADto>>
    {
        private readonly IDevicesAndAssetsUHIARepository _devicesAndAssetsUHIARepository;
        public DevicesAndAssetsUHIASearchQueryHandler(IDevicesAndAssetsUHIARepository devicesAndAssetsUHIARepository)
        {
            _devicesAndAssetsUHIARepository = devicesAndAssetsUHIARepository;
        }
        public async Task<PagedResponse<DevicesAndAssetsUHIADto>> Handle(DevicesAndAssetsUHIASearchQuery request, CancellationToken cancellationToken)
        {
            var res = await DevicesAndAssetsUHIA.Search(_devicesAndAssetsUHIARepository, f =>
            f.ItemListId == request.ItemListId &&
            (!string.IsNullOrEmpty(request.Code) ? f.Code.ToLower().Contains(request.Code.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.UnitRoomEn) && f.UnitRoom != null ? f.UnitRoom.NameEN.ToLower().Contains(request.UnitRoomEn.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.DescriptorAr) ? f.DescriptorAr.ToLower().Contains(request.DescriptorAr.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.DescriptorEn) ? f.DescriptorEn.ToLower().Contains(request.DescriptorEn.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.CategoryEn) && f.Category != null ? f.Category.CategoryEn.ToLower().Contains(request.CategoryEn.ToLower()) : true)
            && (!string.IsNullOrEmpty(request.SubCategoryEn) && f.SubCategory != null ? f.SubCategory.SubCategoryEn.ToLower().Contains(request.SubCategoryEn.ToLower()) : true)
            //
            //&& f.IsDeleted != true
            , request.PageNo, request.PageSize,true, request.OrderBy, request.Ascending);

            var data = res.Data.Select(s => DevicesAndAssetsUHIADto.FromDevicesAndAssetsUHIA(s)).ToList();


            return new PagedResponse<DevicesAndAssetsUHIADto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = data
            };
        }
    }
}
