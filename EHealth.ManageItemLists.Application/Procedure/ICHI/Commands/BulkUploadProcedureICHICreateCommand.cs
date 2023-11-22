using EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands.Validators;
using EHealth.ManageItemLists.Application.Procedure.ICHI.Commands.Validators;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EHealth.ManageItemLists.Application.Procedure.ICHI.Commands
{
    public class BulkUploadProcedureICHICreateCommand : IRequest<byte[]?>, IValidationModel<BulkUploadProcedureICHICreateCommand>
    {
        public IFormFile file { get; set; }
        public BulkUploadProcedureICHICreateCommand(IFormFile file)
        {
            this.file = file;
        }
        public AbstractValidator<BulkUploadProcedureICHICreateCommand> Validator => new BulkUploadPreocedureICHICreateCommandValidator();
    }
}
