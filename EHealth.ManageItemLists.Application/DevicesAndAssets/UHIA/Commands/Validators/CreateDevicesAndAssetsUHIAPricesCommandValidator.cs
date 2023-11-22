using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.Shared.DTOs;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
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
    public class CreateDevicesAndAssetsUHIAPricesCommandValidator : AbstractValidator<CreateDevicesAndAssetsUHIAPricesCommand>
    {
        private readonly IDevicesAndAssetsUHIARepository _devicesAndAssetsUHIAepository;
        private bool _validDevicesAndAssetsUHIA = false;
        public CreateDevicesAndAssetsUHIAPricesCommandValidator(IDevicesAndAssetsUHIARepository devicesAndAssetsUHIARepository)
        {
            _devicesAndAssetsUHIAepository = devicesAndAssetsUHIARepository;

            RuleFor(x => x.DevicesAndAssetsUHIAId).NotNull().NotEmpty();
            RuleFor(x => x.ItemListPrices).NotNull().NotEmpty();
            RuleFor(x => x.DevicesAndAssetsUHIAId).MustAsync(async (DevicesAndAssetsUHIAId, CancellationToken) =>
            {
                try
                {
                    var devicesAndAssetsUHIA = await DevicesAndAssetsUHIA.Get(DevicesAndAssetsUHIAId, _devicesAndAssetsUHIAepository);
                    if (devicesAndAssetsUHIA is not null)
                    {
                        _validDevicesAndAssetsUHIA = true;
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
                .When(x => !string.IsNullOrEmpty(x.DevicesAndAssetsUHIAId.ToString()));

            RuleFor(x => x.ItemListPrices).MustAsync(async (Model, ItemListPrices, CancellationToken) =>
            {
                try
                {
                    var devicesAndAssetsUHIA = await DevicesAndAssetsUHIA.Get(Model.DevicesAndAssetsUHIAId, _devicesAndAssetsUHIAepository);

                    foreach (var item in Model.ItemListPrices)
                    {
                        if (item.EffectiveDateFrom.Date < devicesAndAssetsUHIA.DataEffectiveDateFrom.Date ||
                         (item.EffectiveDateTo.HasValue && devicesAndAssetsUHIA.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > devicesAndAssetsUHIA.DataEffectiveDateTo.Value.Date) ||
                         ((!item.EffectiveDateTo.HasValue) && devicesAndAssetsUHIA.DataEffectiveDateTo.HasValue))
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
            .When(x => x.ItemListPrices != null && x.ItemListPrices.Count() > 0 && _validDevicesAndAssetsUHIA);

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
           .When(x => x.ItemListPrices != null && x.ItemListPrices.Count() > 0 && _validDevicesAndAssetsUHIA);
        }
    }
}
