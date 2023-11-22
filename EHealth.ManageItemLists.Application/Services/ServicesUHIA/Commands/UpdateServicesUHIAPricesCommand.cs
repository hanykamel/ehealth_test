using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands.Validators;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands
{
    public class UpdateServicesUHIAPricesCommand: UpdateServicesUHIAPriceDto, IRequest<bool>, IValidationModel<UpdateServicesUHIAPricesCommand>
    {
        private readonly IServiceUHIARepository _serviceUHIARepository;
        public UpdateServicesUHIAPricesCommand(UpdateServicesUHIAPriceDto request, IServiceUHIARepository serviceUHIARepository)
        {
            ServiceUHIAId = request.ServiceUHIAId;
            ItemListPrices = request.ItemListPrices;
            _serviceUHIARepository = serviceUHIARepository;
        }
        public AbstractValidator<UpdateServicesUHIAPricesCommand> Validator => new UpdateServicesUHIAPricesCommandValidator(_serviceUHIARepository);
    }
}
