using EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands.Validators;
using EHealth.ManageItemLists.Application.Drugs.UHIA.Commands.Validators;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.Commands
{
    public class BulkUploadDrugsUhiaCreateCommand : IRequest<byte[]?>, IValidationModel<BulkUploadDrugsUhiaCreateCommand>
    {
        public IFormFile file { get; set; }
        public BulkUploadDrugsUhiaCreateCommand(IFormFile file)
        {
            this.file = file;
        }
        public AbstractValidator<BulkUploadDrugsUhiaCreateCommand> Validator => new BulkUploadDrugUhiaCreateCommandValidator();
    }
}
