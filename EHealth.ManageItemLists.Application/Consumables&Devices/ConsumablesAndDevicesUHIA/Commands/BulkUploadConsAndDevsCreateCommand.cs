using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Commands.Validators;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Commands
{
    public class BulkUploadConsAndDevsCreateCommand : IRequest<byte[]?>, IValidationModel<BulkUploadConsAndDevsCreateCommand>
    {
        public IFormFile file { get; set; }
        public BulkUploadConsAndDevsCreateCommand(IFormFile file)
        {
            this.file = file;
        }

        public AbstractValidator<BulkUploadConsAndDevsCreateCommand> Validator => new BulkUploadConsAndDevCreateCommandValidator();
    }
}
