using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
using EHealth.ManageItemLists.Application.Shared.DTOs;
using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Infrastructure.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.Commands.Validators
{
    public class UpdateDrugUHIAPricesCommandValidator : AbstractValidator<UpdateDrugUHIAPricesCommand>
    {
        private readonly IDrugsUHIARepository _drugsUHIARepository;
        private bool _validItem = false;
        public UpdateDrugUHIAPricesCommandValidator(IDrugsUHIARepository drugsUHIARepository)
        {
            _drugsUHIARepository = drugsUHIARepository;

            RuleFor(x => x.Id).NotNull().NotEmpty();
            RuleFor(x => x.Id).MustAsync(async (Id, CancellationToken) =>
            {
                try
                {
                    var drugUHIA = await DrugUHIA.Get(Id, _drugsUHIARepository);
                    if (drugUHIA is not null)
                    {
                        _validItem = true;
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
            }).WithErrorCode("DrugUHIANotExist").WithMessage("DrugUHIA with this Id not exist.")
                .When(x => !string.IsNullOrEmpty(x.Id.ToString()));

            RuleFor(x => x.drugPrices).MustAsync(async (Model, ItemListPrices, CancellationToken) =>
            {
                try
                {
                    var drugUHIA = await DrugUHIA.Get(Model.Id, _drugsUHIARepository);

                    foreach (var item in Model.drugPrices)
                    {
                        if (item.EffectiveDateFrom.Date < drugUHIA.DataEffectiveDateFrom.Date ||
                         (item.EffectiveDateTo.HasValue && drugUHIA.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > drugUHIA.DataEffectiveDateTo.Value.Date) ||
                         ((!item.EffectiveDateTo.HasValue) && drugUHIA.DataEffectiveDateTo.HasValue))
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
            .When(x => x.drugPrices != null && x.drugPrices.Count() > 0 && _validItem);

            RuleFor(x => x.drugPrices).Must((Model, CancellationToken) =>
            {
                try
                {
                    var convertedItemLst = new List<DateRangeDto>();
                    foreach (var item in Model.drugPrices)
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
           .When(x => x.drugPrices != null && x.drugPrices.Count() > 0 && _validItem);
        }
    }
}
