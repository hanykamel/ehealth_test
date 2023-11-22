using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands.Validators;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs;
using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Commands.Validators;

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Commands
{
    public class CreateConsAndDevUHIAPricesCommand : CreateConsAndDevUHIAPricesDto, IRequest<Guid>, IValidationModel<CreateConsAndDevUHIAPricesCommand>
    {
        private readonly IConsumablesAndDevicesUHIARepository _consumablesAndDevicesUHIARepository;
        public CreateConsAndDevUHIAPricesCommand(CreateConsAndDevUHIAPricesDto request, IConsumablesAndDevicesUHIARepository consumablesAndDevicesUHIARepository)
        {
            ConsumablesAndDevicesUHIAId = request.ConsumablesAndDevicesUHIAId;
            ItemListPrices = request.ItemListPrices;
            _consumablesAndDevicesUHIARepository = consumablesAndDevicesUHIARepository;
        }

        public AbstractValidator<CreateConsAndDevUHIAPricesCommand> Validator => new CreateConsAndDevUHIAPricesCommandValidator(_consumablesAndDevicesUHIARepository);

    }
}