
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Commands.Validators
{
    public class UpdateConsAndDevUHIABasicDataCommandValidator : AbstractValidator<UpdateConsAndDevUHIABasicDataCommand>
    {
        private readonly IConsumablesAndDevicesUHIARepository _consumablesAndDevicesUHIARepository;
        private bool _valid = false;
        public UpdateConsAndDevUHIABasicDataCommandValidator(IConsumablesAndDevicesUHIARepository consumablesAndDevicesUHIARepository)
        {
            _consumablesAndDevicesUHIARepository = consumablesAndDevicesUHIARepository;

            RuleFor(x => x.Id).MustAsync(async (Id, CancellationToken) =>
            {
                try
                {
                    var consumablesAndDevicesUHIA = await Domain.ConsumablesAndDevices.ConsumablesAndDevicesUHIA.Get(Id, _consumablesAndDevicesUHIARepository);
                    if (consumablesAndDevicesUHIA is not null)
                    {
                        _valid = true;
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
            }).WithErrorCode("ConsumablesAndDevicesUHIANotExist").WithMessage("ConsumablesAndDevicesUHIA with Id not exist.")
                .When(x => !string.IsNullOrEmpty(x.Id.ToString()));

            RuleFor(x => new { x.DataEffectiveDateFrom, x.DataEffectiveDateTo }).MustAsync(async (Model, tmp, CancellationToken) =>
            {
                try
                {
                    var consumablesAndDevicesUHIA = await Domain.ConsumablesAndDevices.ConsumablesAndDevicesUHIA.Get(Model.Id, _consumablesAndDevicesUHIARepository);
                    var notDeletedItems = consumablesAndDevicesUHIA.ItemListPrices.Where(x => x.IsDeleted == false).ToList();
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
            .When(x => _valid);
        }
    }
}
