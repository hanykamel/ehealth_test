using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Commands.Validators;
using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Commands
{
    public class UpdateConsAndDevUHIABasicDataCommand : UpdateConsAndDevUHIABasicDataDto, IRequest<bool>, IValidationModel<UpdateConsAndDevUHIABasicDataCommand>
    {
        private readonly IConsumablesAndDevicesUHIARepository _consumablesAndDevicesUHIARepository;
        public UpdateConsAndDevUHIABasicDataCommand(UpdateConsAndDevUHIABasicDataDto request, IConsumablesAndDevicesUHIARepository consumablesAndDevicesUHIARepository)
        {
            Id = request.Id;
            EHealthCode = request.EHealthCode;
            UHIAId = request.UHIAId;
            ShortDescAr = request.ShortDescAr;
            ShortDescEn = request.ShortDescEn;
            ServiceCategoryId = request.ServiceCategoryId;
            ServiceSubCategoryId = request.ServiceSubCategoryId;
            //ItemListId = request.ItemListId;
            DataEffectiveDateFrom = request.DataEffectiveDateFrom;
            DataEffectiveDateTo = request.DataEffectiveDateTo;
            UnitOfMeasureId = request.UnitOfMeasureId;

            _consumablesAndDevicesUHIARepository = consumablesAndDevicesUHIARepository;
        }
        public AbstractValidator<UpdateConsAndDevUHIABasicDataCommand> Validator => new UpdateConsAndDevUHIABasicDataCommandValidator(_consumablesAndDevicesUHIARepository);
    }
}
