using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.Shared.DTOs;
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
    public class UpdateDevicesAndAssetsUHIAPricesCommandValidator : AbstractValidator<UpdateDevicesAndAssetsUHIAPricesCommand>
    {
        private readonly IDevicesAndAssetsUHIARepository _deviceAndAssetUHIARepository;
        private bool _validDeviceAndAssetUHIA = false;
        public UpdateDevicesAndAssetsUHIAPricesCommandValidator(IDevicesAndAssetsUHIARepository deviceAndAssetUHIARepository)
        {
            _deviceAndAssetUHIARepository = deviceAndAssetUHIARepository;

            RuleFor(x => x.DevicesAndAssetsUHIAId).NotNull().NotEmpty();
            RuleFor(x => x.ItemListPrices).NotNull().NotEmpty();
            RuleFor(x => x.DevicesAndAssetsUHIAId).MustAsync(async (DevicesAndAssetUHIAId, CancellationToken) =>
            {
                try
                {
                    var devicesAndAssetUHIA = await DevicesAndAssetsUHIA.Get(DevicesAndAssetUHIAId, _deviceAndAssetUHIARepository);
                    if (devicesAndAssetUHIA is not null)
                    {
                        _validDeviceAndAssetUHIA = true;
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
            }).WithErrorCode("DevicesAndAssetUHIANotExist").WithMessage("DevicesAndAssetUHIA with DevicesAndAssetUHIAId not exist.")
                .When(x => !string.IsNullOrEmpty(x.DevicesAndAssetsUHIAId.ToString()));

            RuleFor(x => x.ItemListPrices).MustAsync(async (Model, ItemListPrices, CancellationToken) =>
            {
                try
                {
                    var devicesAndAssetUHIA = await DevicesAndAssetsUHIA.Get(Model.DevicesAndAssetsUHIAId, _deviceAndAssetUHIARepository);

                    foreach (var item in Model.ItemListPrices)
                    {
                        if (item.EffectiveDateFrom.Date < devicesAndAssetUHIA.DataEffectiveDateFrom.Date ||
                         (item.EffectiveDateTo.HasValue && devicesAndAssetUHIA.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > devicesAndAssetUHIA.DataEffectiveDateTo.Value.Date) ||
                         ((!item.EffectiveDateTo.HasValue) && devicesAndAssetUHIA.DataEffectiveDateTo.HasValue))
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
            .When(x => x.ItemListPrices != null && x.ItemListPrices.Count() > 0 && _validDeviceAndAssetUHIA);

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
           .When(x => x.ItemListPrices != null && x.ItemListPrices.Count() > 0 && _validDeviceAndAssetUHIA);
        }
    }
}
