using EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands.Validators;
using EHealth.ManageItemLists.Application.DoctorFees.UHIA.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands
{
    public class UpdateDoctorFeesUHIABasicDataCommand : UpdateDoctoerFeesUHIABasicDataDto, IRequest<bool>, IValidationModel<UpdateDoctorFeesUHIABasicDataCommand>
    {
        private readonly IDoctorFeesUHIARepository _doctorFeesUHIARepository;
        public UpdateDoctorFeesUHIABasicDataCommand(UpdateDoctoerFeesUHIABasicDataDto request, IDoctorFeesUHIARepository doctorFeesUHIARepository)
        {
            Id = request.Id;
            EHealthCode = request.EHealthCode;
            DescriptorAr= request.DescriptorAr;
            DescriptorEn= request.DescriptorEn;
            PackageCompexityClassificationId= request.PackageCompexityClassificationId;
            DataEffectiveDateFrom= request.DataEffectiveDateFrom;
            DataEffectiveDateTo= request.DataEffectiveDateTo;
            _doctorFeesUHIARepository = doctorFeesUHIARepository;
            
        }
        public AbstractValidator<UpdateDoctorFeesUHIABasicDataCommand> Validator => new UpdateDoctorFeesUHIABasicDataCommandValidator(_doctorFeesUHIARepository);
    }
}
