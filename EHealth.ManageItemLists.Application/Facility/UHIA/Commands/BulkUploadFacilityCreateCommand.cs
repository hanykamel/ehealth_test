using EHealth.ManageItemLists.Application.Facility.UHIA.Commands.Validators;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EHealth.ManageItemLists.Application.Facility.UHIA.Commands
{
    public class BulkUploadFacilityCreateCommand : IRequest<byte[]?>, IValidationModel<BulkUploadFacilityCreateCommand>
    {
        public IFormFile file { get; set; }
        public BulkUploadFacilityCreateCommand(IFormFile file)
        {
            this.file = file;
        }
        public AbstractValidator<BulkUploadFacilityCreateCommand> Validator => new BulkUploadFacilityCreateCommandValidator();
    }
}
