using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.Shared.DTOs;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using FluentValidation;

namespace EHealth.ManageItemLists.Application.Resource.UHIA.Commands.Validators
{
    public class UpdateResourceUHIAPricesCommandValidator : AbstractValidator<UpdateResourceUHIAPricesCommand>
    {
        private readonly IResourceUHIARepository _resourceUHIARepository;
        private bool _validResourceUHIA = false;
        public UpdateResourceUHIAPricesCommandValidator(IResourceUHIARepository resourceUHIARepository)
        {
            _resourceUHIARepository = resourceUHIARepository;

            RuleFor(x => x.ResourceUHIAId).MustAsync(async (ResourceUHIAId, CancellationToken) =>
            {
                try
                {
                    var resourceUHIA = await ResourceUHIA.Get(ResourceUHIAId, _resourceUHIARepository);
                    if (resourceUHIA is not null)
                    {
                        _validResourceUHIA = true;
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
            }).WithErrorCode("ResourceUHIANotExist").WithMessage("ResourceUHIA with ResourceUHIAId not exist.")
                .When(x => !string.IsNullOrEmpty(x.ResourceUHIAId.ToString()));

            RuleFor(x => x.ResourceItemPrices).MustAsync(async (Model, ResourceItemPrices, CancellationToken) =>
            {
                try
                {
                    var doctorUHIA = await ResourceUHIA.Get(Model.ResourceUHIAId, _resourceUHIARepository);

                    foreach (var item in Model.ResourceItemPrices)
                    {
                        if (item.EffectiveDateFrom.Date < doctorUHIA.DataEffectiveDateFrom.Date ||
                         (item.EffectiveDateTo.HasValue && doctorUHIA.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > doctorUHIA.DataEffectiveDateTo.Value.Date) ||
                         ((!item.EffectiveDateTo.HasValue) && doctorUHIA.DataEffectiveDateTo.HasValue))
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
      .When(x => x.ResourceItemPrices != null && x.ResourceItemPrices.Count() > 0 && _validResourceUHIA);

            RuleFor(x => x.ResourceItemPrices).Must((Model, CancellationToken) =>
            {
                try
                {
                    var convertedItemLst = new List<DateRangeDto>();
                    foreach (var item in Model.ResourceItemPrices)
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
          .When(x => x.ResourceItemPrices != null && x.ResourceItemPrices.Count() > 0 && _validResourceUHIA);
        }
    }
}
