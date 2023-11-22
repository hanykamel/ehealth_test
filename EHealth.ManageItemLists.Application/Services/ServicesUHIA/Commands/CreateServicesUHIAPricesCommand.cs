using EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands.Validators;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands.Validators;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands
{
    public class CreateServicesUHIAPricesCommand : CreateServicesUHIAPriceDto, IRequest<Guid>, IValidationModel<CreateServicesUHIAPricesCommand>
    {
        private readonly IServiceUHIARepository _serviceUHIARepository;
        public CreateServicesUHIAPricesCommand(CreateServicesUHIAPriceDto request, IServiceUHIARepository serviceUHIARepository)
        {
            ServiceUHIAId = request.ServiceUHIAId;
            ItemListPrices = request.ItemListPrices;
            _serviceUHIARepository = serviceUHIARepository;
        }

        public AbstractValidator<CreateServicesUHIAPricesCommand> Validator => new CreateServicesUHIAPricesCommandValidator(_serviceUHIARepository);

    }
}
