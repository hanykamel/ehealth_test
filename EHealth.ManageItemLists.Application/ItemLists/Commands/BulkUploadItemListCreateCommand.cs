using EHealth.ManageItemLists.Application.ItemLists.Commands.Validators;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EHealth.ManageItemLists.Application.ItemLists.Commands
{
    public class BulkUploadItemListCreateCommand : IRequest<byte[]?>, IValidationModel<BulkUploadItemListCreateCommand>
    {
        public IFormFile file { get; set; }
        public BulkUploadItemListCreateCommand(IFormFile file)
        {
            this.file = file;
        }
        public AbstractValidator<BulkUploadItemListCreateCommand> Validator => new BulkUploadUtemListCreateCommandValidator();
    }
}
