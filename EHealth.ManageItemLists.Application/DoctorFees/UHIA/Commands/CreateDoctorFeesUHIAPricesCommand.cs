using EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands.Validators;
using EHealth.ManageItemLists.Application.DoctorFees.UHIA.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands
{
    public class CreateDoctorFeesUHIAPricesCommand : CreateDoctorFeesUHIAPriceDto, IRequest<Guid>, IValidationModel<CreateDoctorFeesUHIAPricesCommand>
    {
        private readonly IDoctorFeesUHIARepository _doctorFeesUHIARepository;
        public CreateDoctorFeesUHIAPricesCommand(CreateDoctorFeesUHIAPriceDto request, IDoctorFeesUHIARepository doctorFeesUHIARepository)
        {
            DoctorFeesUHIAId = request.DoctorFeesUHIAId;
            ItemListPrices = request.ItemListPrices;
            _doctorFeesUHIARepository = doctorFeesUHIARepository;
        }
        public AbstractValidator<CreateDoctorFeesUHIAPricesCommand> Validator => new CreateDoctorFeesUHIAPricesCommandValidator(_doctorFeesUHIARepository);
    }
}
