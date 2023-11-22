using EHealth.ManageItemLists.Application.Resource.UHIA.Commands.Validators;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EHealth.ManageItemLists.Application.Resource.UHIA.Commands
{
    public class BulkUploadResourceUhiaCreateCommand : IRequest<byte[]?>, IValidationModel<BulkUploadResourceUhiaCreateCommand>
    {
        public IFormFile file { get; set; }
        public BulkUploadResourceUhiaCreateCommand(IFormFile file)
        {
            this.file = file;
        }
        public AbstractValidator<BulkUploadResourceUhiaCreateCommand> Validator => new BulkUploadResourceUhiaCreateCommandValidator();
    }
}
