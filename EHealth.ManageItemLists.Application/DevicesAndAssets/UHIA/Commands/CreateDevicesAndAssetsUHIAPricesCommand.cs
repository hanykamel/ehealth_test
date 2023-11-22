using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Commands.Validators;
using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands.Validators;
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

namespace EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Commands
{
    public class CreateDevicesAndAssetsUHIAPricesCommand : CreateDevicesAndAssetsUHIAPriceDto, IRequest<Guid>, IValidationModel<CreateDevicesAndAssetsUHIAPricesCommand>
    {
        private readonly IDevicesAndAssetsUHIARepository _devicesAndAssetsUHIARepository;
        public CreateDevicesAndAssetsUHIAPricesCommand(CreateDevicesAndAssetsUHIAPriceDto request, IDevicesAndAssetsUHIARepository devicesAndAssetsUHIARepository)
        {
            DevicesAndAssetsUHIAId = request.DevicesAndAssetsUHIAId;
            ItemListPrices = request.ItemListPrices;
            _devicesAndAssetsUHIARepository = devicesAndAssetsUHIARepository;
        }

        public AbstractValidator<CreateDevicesAndAssetsUHIAPricesCommand> Validator => new CreateDevicesAndAssetsUHIAPricesCommandValidator(_devicesAndAssetsUHIARepository);

    }
}
