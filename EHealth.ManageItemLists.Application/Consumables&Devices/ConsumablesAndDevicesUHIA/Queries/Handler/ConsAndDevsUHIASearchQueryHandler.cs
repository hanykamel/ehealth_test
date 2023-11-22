using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs;
using EHealth.ManageItemLists.Application.ItemLists.DTOs;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Queries.Handler
{
    public class ConsAndDevsUHIASearchQueryHandler : IRequestHandler<ConsAndDevUHIASearchQuery, PagedResponse<ConsAndDevDto>>
    {
        private readonly IConsumablesAndDevicesUHIARepository _consumablesAndDevicesUHIARepository;
       
        public ConsAndDevsUHIASearchQueryHandler(IConsumablesAndDevicesUHIARepository consumablesAndDevicesUHIARepository)
        {
            _consumablesAndDevicesUHIARepository = consumablesAndDevicesUHIARepository;   
        }
        public async Task<PagedResponse<ConsAndDevDto>> Handle(ConsAndDevUHIASearchQuery request, CancellationToken cancellationToken)
        {
            var res = await Domain.ConsumablesAndDevices.ConsumablesAndDevicesUHIA.Search(_consumablesAndDevicesUHIARepository, f => f.ItemListId == request.ItemListId &&
            (!string.IsNullOrEmpty(request.EHealthCode) ? f.EHealthCode.ToLower().Contains(request.EHealthCode.ToLower()) : true)
           && (!string.IsNullOrEmpty(request.UHIAId) ? f.UHIAId.ToLower().Contains(request.UHIAId.ToLower()) : true)
            //&& (!string.IsNullOrEmpty(request.ShortDescriptionAr) && !string.IsNullOrEmpty(f.ShortDescriptorAr) ? f.ShortDescriptorAr.ToLower().Contains(request.ShortDescriptionAr.ToLower()) : true)
           && (!string.IsNullOrEmpty(request.ShortDescriptionAr) ? f.ShortDescriptorAr.ToLower().Contains(request.ShortDescriptionAr.ToLower()) : true)
           && (!string.IsNullOrEmpty(request.ShortDescriptionEn) ? f.ShortDescriptorEn.ToLower().Contains(request.ShortDescriptionEn.ToLower()) : true)
            , request.PageNo, request.PageSize,request.EnablePagination, request.OrderBy, request.Ascending);

            var data = res.Data.Select(s => ConsAndDevDto.FromConsAndDevsUHIA(s)).ToList();

            return new PagedResponse<ConsAndDevDto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = data           
            };
        }

     
    }
}
