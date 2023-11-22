using EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands.Validators;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands
{
    public class BulkUploadDrFeesCreateCommand : IRequest<byte[]?>, IValidationModel<BulkUploadDrFeesCreateCommand>
    {
        public IFormFile file { get; set; }
        public BulkUploadDrFeesCreateCommand(IFormFile file)
        {
            this.file = file;
        }
        public AbstractValidator<BulkUploadDrFeesCreateCommand> Validator => new BulkUploadDrFeesCreateCommandValidator();
    }
}
