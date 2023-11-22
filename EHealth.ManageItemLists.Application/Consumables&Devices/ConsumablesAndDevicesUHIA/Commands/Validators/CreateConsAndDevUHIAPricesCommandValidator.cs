using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
using EHealth.ManageItemLists.Application.Shared.DTOs;
using EHealth.ManageItemLists.Domain.ConsumablesAndDevices;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Commands.Validators
{
    public class CreateConsAndDevUHIAPricesCommandValidator : AbstractValidator<CreateConsAndDevUHIAPricesCommand>
    {
        private readonly IConsumablesAndDevicesUHIARepository _consumablesAndDevicesUHIARepository;
        private bool _valid = false;
        public CreateConsAndDevUHIAPricesCommandValidator(IConsumablesAndDevicesUHIARepository consumablesAndDevicesUHIARepository)
        {
            _consumablesAndDevicesUHIARepository = consumablesAndDevicesUHIARepository;

            RuleFor(x => x.ConsumablesAndDevicesUHIAId).NotNull().NotEmpty();
            RuleFor(x => x.ItemListPrices).NotNull().NotEmpty();
            RuleFor(x => x.ConsumablesAndDevicesUHIAId).MustAsync(async (ConsumablesAndDevicesUHIAId, CancellationToken) =>
            {
                try
                {
                    var consumablesAndDevicesUHIA = await Domain.ConsumablesAndDevices.ConsumablesAndDevicesUHIA.Get(ConsumablesAndDevicesUHIAId, _consumablesAndDevicesUHIARepository);
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
            }).WithErrorCode("ConsumablesAndDevicesUHIAExist").WithMessage("ConsumablesAndDevicesUHIA with ConsumablesAndDevicesUHIAId not exist.")
                .When(x => !string.IsNullOrEmpty(x.ConsumablesAndDevicesUHIAId.ToString()));

            RuleFor(x => x.ItemListPrices).MustAsync(async (Model, ItemListPrices, CancellationToken) =>
            {
                try
                {
                    var consumablesAndDevicesUHIA = await Domain.ConsumablesAndDevices.ConsumablesAndDevicesUHIA.Get(Model.ConsumablesAndDevicesUHIAId, _consumablesAndDevicesUHIARepository);

                    foreach (var item in Model.ItemListPrices)
                    {
                        if (item.EffectiveDateFrom.Date < consumablesAndDevicesUHIA.DataEffectiveDateFrom.Date ||
                         (item.EffectiveDateTo.HasValue && consumablesAndDevicesUHIA.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > consumablesAndDevicesUHIA.DataEffectiveDateTo.Value.Date) ||
                         ((!item.EffectiveDateTo.HasValue) && consumablesAndDevicesUHIA.DataEffectiveDateTo.HasValue))
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
            .When(x => x.ItemListPrices != null && x.ItemListPrices.Count() > 0 && _valid);

            RuleFor(x => x.ItemListPrices).Must((Model, CancellationToken) =>
            {
                try
                {
                    var convertedItemLst = new List<DateRangeDto>();
                    foreach (var item in Model.ItemListPrices)
                    {
                        var convertedItem = new DateRangeDto
                        {
                            Start = item.EffectiveDateFrom.Date,
                            End = item.EffectiveDateTo.HasValue ? item.EffectiveDateTo.Value.Date : null
                        };
                        convertedItemLst.Add(convertedItem);
                    }

                    if (DateAndTimeOperations.DoesNotOverlap(convertedItemLst))
                    {
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
            }).WithErrorCode("ItemManagement_MSG_27").WithMessage("The dates overlap with those already specified. Please enter additional dates.")
           .When(x => x.ItemListPrices != null && x.ItemListPrices.Count() > 0 && _valid); ;
        }
    }
}
