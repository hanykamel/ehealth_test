using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Infrastructure.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Commands.Validators
{
   public class UpdateDevicesAndAssetsUHIABasicDataCommandValidator : AbstractValidator<UpdateDevicesAndAssetsUHIABasicDataCommand>
    {
        private readonly IDevicesAndAssetsUHIARepository _devicesAndAssetsUHIARepository;
        private bool _validDevicesAndAssetsUHIAUHIA = false;
        public UpdateDevicesAndAssetsUHIABasicDataCommandValidator(IDevicesAndAssetsUHIARepository devicesAndAssetsUHIARepository)
        {
            _devicesAndAssetsUHIARepository = devicesAndAssetsUHIARepository;

            RuleFor(x => x.Id).MustAsync(async (DevicesAndAssetsUHIAId, CancellationToken) =>
            {
                try
                {
                    var devicesAndAssetsUHIA = await DevicesAndAssetsUHIA.Get( DevicesAndAssetsUHIAId, _devicesAndAssetsUHIARepository);
                    if (devicesAndAssetsUHIA is not null)
                    {
                        _validDevicesAndAssetsUHIAUHIA = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }).WithErrorCode("DevicesAndAssetsUHIANotExist").WithMessage("DevicesAndAssetsUHIA with DevicesAndAssetsUHIAId not exist.")
                .When(x => !string.IsNullOrEmpty(x.Id.ToString()));

            RuleFor(x => new { x.DataEffectiveDateFrom, x.DataEffectiveDateTo }).MustAsync(async (Model, ItemListPrices, CancellationToken) =>
            {
                try
                {
                    var devicesAndAssetsUHIA = await DevicesAndAssetsUHIA.Get(Model.Id, _devicesAndAssetsUHIARepository);
                    var notDeletedItems = devicesAndAssetsUHIA.ItemListPrices.Where(x => x.IsDeleted == false).ToList();
                    foreach (var item in notDeletedItems)
                    {
                        if (item.EffectiveDateFrom.Date < Model.DataEffectiveDateFrom.Date ||
                         (item.EffectiveDateTo.HasValue && Model.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > Model.DataEffectiveDateTo.Value.Date) ||
                         ((!item.EffectiveDateTo.HasValue) && Model.DataEffectiveDateTo.HasValue))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }).WithErrorCode("ItemManagement_MSG_10").WithMessage("Price's effective dates must be within the bounds of basic item data effective dates.")
           .When(x => _validDevicesAndAssetsUHIAUHIA);
        }
    }
}
