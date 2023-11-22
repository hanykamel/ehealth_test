using EHealth.ManageItemLists.Application.DoctorFees.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands.Validators;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using EHealth.ManageItemLists.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands.Validators;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands
{
    public class UpdateDoctorFeesUHIAPricesCommand : UpdateDoctorFeesUHIAPriceDto, IRequest<bool>, IValidationModel<UpdateDoctorFeesUHIAPricesCommand>
    {
        private readonly IDoctorFeesUHIARepository _doctorFeesUHIARepository;
        public UpdateDoctorFeesUHIAPricesCommand(UpdateDoctorFeesUHIAPriceDto request, IDoctorFeesUHIARepository doctorFeesUHIARepository)
        {
            DoctorFeesUHIAId = request.DoctorFeesUHIAId;
            ItemListPrices = request.ItemListPrices;
            _doctorFeesUHIARepository = doctorFeesUHIARepository;
        }
        public AbstractValidator<UpdateDoctorFeesUHIAPricesCommand> Validator => new UpdateDoctorFeesUHIAPricesCommandValidator(_doctorFeesUHIARepository);
    }
}
