using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Commands.Validators;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Commands
{
    public class BulkUploadDevicesAndAssetsCreateCommand : IRequest<byte[]?>, IValidationModel<BulkUploadDevicesAndAssetsCreateCommand>
    {
        public IFormFile file { get; set; }
        public BulkUploadDevicesAndAssetsCreateCommand(IFormFile file)
        {
            this.file = file;
        }
        public AbstractValidator<BulkUploadDevicesAndAssetsCreateCommand> Validator => new BulkUploadDevicesAndAssetsCreateCommandValidator();
    }
}
