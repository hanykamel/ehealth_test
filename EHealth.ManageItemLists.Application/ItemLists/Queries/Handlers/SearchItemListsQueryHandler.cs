using EHealth.ManageItemLists.Application.ItemLists.Commands;
using EHealth.ManageItemLists.Application.ItemLists.DTOs;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using EHealth.ManageItemLists.Domain.ConsumablesAndDevices;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.DoctorFees.UHIA;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.ItemListTypes;
using EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using EHealth.ManageItemLists.Infrastructure.Repositories;
using MediatR;


namespace EHealth.ManageItemLists.Application.ItemLists.Queries.Handlers
{
    public class SearchItemListsQueryHandler : IRequestHandler<SearchItemListQuery, PagedResponse<ItemListDto>>
    {
        
        private readonly IItemListRepository _itemListsRepository;
        private readonly IServiceUHIARepository _serviceUHIARepository;
        private readonly IConsumablesAndDevicesUHIARepository _consumablesAndDevicesUHIARepository;
        private readonly IProcedureICHIRepository _procedureICHIRepository;
        private readonly IDevicesAndAssetsUHIARepository _devicesAndAssetsUHIARepository;
        private readonly IFacilityUHIARepository _facilityUHIARepository;
        private readonly IResourceUHIARepository _resourceUHIARepository;
        private readonly IDoctorFeesUHIARepository _doctorFeesUHIARepository;
        public SearchItemListsQueryHandler( IItemListRepository itemListsRepository, IServiceUHIARepository serviceUHIARepository,
            IConsumablesAndDevicesUHIARepository consumablesAndDevicesUHIARepository, IProcedureICHIRepository procedureICHIRepository,
            IDevicesAndAssetsUHIARepository devicesAndAssetsUHIARepository, IFacilityUHIARepository facilityUHIARepository,
            IResourceUHIARepository resourceUHIARepository, IDoctorFeesUHIARepository doctorFeesUHIARepository)
        {
            _itemListsRepository = itemListsRepository;
            _serviceUHIARepository = serviceUHIARepository;
            _consumablesAndDevicesUHIARepository = consumablesAndDevicesUHIARepository;
            _procedureICHIRepository = procedureICHIRepository;
            _devicesAndAssetsUHIARepository = devicesAndAssetsUHIARepository;
            _facilityUHIARepository = facilityUHIARepository;
            _resourceUHIARepository = resourceUHIARepository;
            _doctorFeesUHIARepository = doctorFeesUHIARepository;
        }

        public async Task<PagedResponse<ItemListDto>> Handle(SearchItemListQuery request, CancellationToken cancellationToken)
        {
            var output = await ItemList.Search(_itemListsRepository,f =>
            (!string.IsNullOrEmpty(request.Code) ? f.Code.ToLower().Contains(request.Code.ToLower()) : true) &&
            (request.ItemListTypeId.HasValue ? f.ItemListSubtype.ItemListTypeId == request.ItemListTypeId: true) &&
            (request.ItemListSubtypeId.HasValue ? f.ItemListSubtypeId == request.ItemListSubtypeId : true) &&
            (!string.IsNullOrEmpty(request.NameAr) ? f.NameAr.ToLower().Contains(request.NameAr.ToLower()) : true) &&
            (!string.IsNullOrEmpty(request.NameEN) ? f.NameEN.ToLower().Contains(request.NameEN.ToLower()) : true) &&
            (request.Id.HasValue ? f.Id == request.Id : true), request.PageNo,
            request.PageSize, request.OrderBy, request.Ascending, request.EnablePagination);



            return new PagedResponse<ItemListDto>
            {
                PageNumber = output.PageNumber,
                PageSize = output.PageSize,
                TotalCount = output.TotalCount,
                Data = output.Data.Select(s => ItemListDto.FromItemList(s)).ToList()
            };
        }


    }
}
