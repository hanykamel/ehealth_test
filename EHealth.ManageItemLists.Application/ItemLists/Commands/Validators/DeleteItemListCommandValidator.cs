using EHealth.ManageItemLists.Domain.ConsumablesAndDevices;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.DoctorFees.UHIA;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using FluentValidation;

namespace EHealth.ManageItemLists.Application.ItemLists.Commands.Validators
{
    public class DeleteItemListCommandValidator : AbstractValidator<DeleteItemListCommand>
    {
        private readonly IItemListRepository _itemListRepository;
        private readonly IServiceUHIARepository _serviceUHIARepository;
        private readonly IConsumablesAndDevicesUHIARepository _consumablesAndDevicesUHIARepository;
        private readonly IProcedureICHIRepository _procedureICHIRepository;
        private readonly IDevicesAndAssetsUHIARepository _devicesAndAssetsUHIARepository;
        private readonly IFacilityUHIARepository _facilityUHIARepository;
        private readonly IResourceUHIARepository _resourceUHIARepository;
        private readonly IDoctorFeesUHIARepository _doctorFeesUHIARepository;
        private bool _validItemList = false;
        public DeleteItemListCommandValidator(IItemListRepository itemListRepository, IServiceUHIARepository serviceUHIARepository,
            IConsumablesAndDevicesUHIARepository consumablesAndDevicesUHIARepository, IProcedureICHIRepository procedureICHIRepository,
            IDevicesAndAssetsUHIARepository devicesAndAssetsUHIARepository, IFacilityUHIARepository facilityUHIARepository,
            IResourceUHIARepository resourceUHIARepository, IDoctorFeesUHIARepository doctorFeesUHIARepository)
        {
            _itemListRepository = itemListRepository;
            _serviceUHIARepository = serviceUHIARepository;
            _consumablesAndDevicesUHIARepository = consumablesAndDevicesUHIARepository;
            _procedureICHIRepository = procedureICHIRepository;
            _devicesAndAssetsUHIARepository = devicesAndAssetsUHIARepository;
            _facilityUHIARepository = facilityUHIARepository;
            _resourceUHIARepository = resourceUHIARepository;
            _doctorFeesUHIARepository = doctorFeesUHIARepository;
            RuleFor(x => x.Id).MustAsync(async (listId, CancellationToken) =>
            {
                try
                {


                    var serviceUHIAres = await ServiceUHIA.Search(_serviceUHIARepository, x => x.IsDeleted == false && x.ItemListId == listId, 0, 0, false, null, null);
                    if (serviceUHIAres.Data.Count>0)
                    {
                        _validItemList = false;
                        return false;
                    }
                    var consAndDevsUHIA = await ConsumablesAndDevicesUHIA.Search(_consumablesAndDevicesUHIARepository, x => x.IsDeleted == false && x.ItemListId == listId, 0, 0, false, null, null);
                    if (consAndDevsUHIA.Data.Count > 0)
                    {
                        _validItemList = false;
                        return false;
                    }
                    var consumablesAndDevicesUHIA = await ProcedureICHI.Search(_procedureICHIRepository, x => x.IsDeleted == false && x.ItemListId == listId, 0, 0, false, null, null);
                    if (consumablesAndDevicesUHIA.Data.Count > 0)
                    {
                        _validItemList = false;
                        return false;
                    }
                    var DeviceAndAssetsUHIA = await DevicesAndAssetsUHIA.Search(_devicesAndAssetsUHIARepository, x => x.IsDeleted == false && x.ItemListId == listId, 0, 0, false, null, null);
                    if (DeviceAndAssetsUHIA.Data.Count > 0)
                    {
                        _validItemList = false;
                        return false;
                    }
                    var facilityUHIA = await FacilityUHIA.Search(_facilityUHIARepository, x => x.IsDeleted == false && x.ItemListId == listId, 0, 0, false, null, null);
                    if (facilityUHIA.Data.Count > 0)
                    {
                        _validItemList = false;
                        return false;
                    }
                    var resourceUHIA = await ResourceUHIA.Search(_resourceUHIARepository, x => x.IsDeleted == false && x.ItemListId == listId, 0, 0, false, null, null);
                    if (resourceUHIA.Data.Count > 0)
                    {
                        _validItemList = false;
                        return false;
                    }
                    var DrFeesUHIA = await DoctorFeesUHIA.Search(_doctorFeesUHIARepository, x => x.IsDeleted == false && x.ItemListId == listId, 0, 0, false, null, null);
                    if (DrFeesUHIA.Data.Count > 0)
                    {
                        _validItemList = false;
                        return false;
                    }
                    return true;

                }
                catch (Exception ex)
                {
                    return false;
                }
            }).WithErrorCode("ItemManagement_MSG_03");


        }
    }
}
