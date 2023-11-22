using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands.Validators;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
using FluentValidation;
using MediatR;
using EHealth.ManageItemLists.Application.ItemLists.Commands.Validators;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Application.ItemLists.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using EHealth.ManageItemLists.Infrastructure.Repositories;

namespace EHealth.ManageItemLists.Application.ItemLists.Commands
{
    public class DeleteItemListCommand :DeleteItemListDTO, IRequest<bool>, IValidationModel<DeleteItemListCommand>
    {

        private readonly IItemListRepository _repository;
        private readonly IServiceUHIARepository _serviceUHIARepository;
        private readonly IConsumablesAndDevicesUHIARepository _consumablesAndDevicesUHIARepository;
        private readonly IProcedureICHIRepository _procedureICHIRepository;
        private readonly IDevicesAndAssetsUHIARepository _devicesAndAssetsUHIARepository;
        private readonly IFacilityUHIARepository _facilityUHIARepository;
        private readonly IResourceUHIARepository _resourceUHIARepository;
        private readonly IDoctorFeesUHIARepository _doctorFeesUHIARepository;
        public DeleteItemListCommand(DeleteItemListDTO deleteItemListDTO, IItemListRepository itemListRepository,
            IServiceUHIARepository serviceUHIARepository,IConsumablesAndDevicesUHIARepository consumablesAndDevicesUHIARepository,
            IProcedureICHIRepository procedureICHIRepository,IDevicesAndAssetsUHIARepository devicesAndAssetsUHIARepository, IFacilityUHIARepository facilityUHIARepository,
            IResourceUHIARepository resourceUHIARepository, IDoctorFeesUHIARepository doctorFeesUHIARepository)
        {
            _repository = itemListRepository;
            _serviceUHIARepository = serviceUHIARepository;
            _consumablesAndDevicesUHIARepository  = consumablesAndDevicesUHIARepository;
            _procedureICHIRepository = procedureICHIRepository;
            _devicesAndAssetsUHIARepository = devicesAndAssetsUHIARepository;
            _facilityUHIARepository = facilityUHIARepository;
            _resourceUHIARepository = resourceUHIARepository;
            _doctorFeesUHIARepository = doctorFeesUHIARepository;
            Id = deleteItemListDTO.Id;
        }

        public AbstractValidator<DeleteItemListCommand> Validator => new DeleteItemListCommandValidator(_repository,_serviceUHIARepository,_consumablesAndDevicesUHIARepository,
            _procedureICHIRepository,_devicesAndAssetsUHIARepository,_facilityUHIARepository,_resourceUHIARepository,_doctorFeesUHIARepository);

    }
}
