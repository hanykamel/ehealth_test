using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
using FluentValidation;

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Commands.Validators
{
    public class BulkUploadConsAndDevCreateCommandValidator : AbstractValidator<BulkUploadConsAndDevsCreateCommand>
    {

        public BulkUploadConsAndDevCreateCommandValidator()
        {

            RuleFor(x => x.file).MustAsync(async (file, CancellationToken) =>
            {
                try
                {
                    var splitFileName = file.FileName.Split('.');
                    var extension = splitFileName[splitFileName.Count() - 1];
                    if (extension != "xlsx")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }).WithErrorCode("ItemManagement_MSG_34").WithMessage("Attached file has a different extension than the required extension (required xlsx extension).");
        }
    }
}
