using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.Shared.DTOs;
using EHealth.ManageItemLists.Domain.DoctorFees.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using FluentValidation;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands.Validators
{
    public class UpdateDoctorFeesUHIAPricesCommandValidator : AbstractValidator<UpdateDoctorFeesUHIAPricesCommand>
    {
        private readonly IDoctorFeesUHIARepository _doctorFeesUHIARepository;
        private bool _validDrFeesUHIA = false;
        public UpdateDoctorFeesUHIAPricesCommandValidator(IDoctorFeesUHIARepository doctorFeesUHIARepository)
        {
            _doctorFeesUHIARepository = doctorFeesUHIARepository;
            RuleFor(x => x.ItemListPrices).NotNull().NotEmpty();
            RuleFor(x => x.DoctorFeesUHIAId).MustAsync(async (DoctorFeesUHIAId, CancellationToken) =>
            {
                try
                {
                    var doctorUHIA = await DoctorFeesUHIA.Get(DoctorFeesUHIAId, _doctorFeesUHIARepository);
                    if (doctorUHIA is not null)
                    {
                        _validDrFeesUHIA = true;
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
            }).WithErrorCode("doctorUHIANotExist").WithMessage("doctorUHIA with DoctorFeesUHIAId not exist.")
               .When(x => !string.IsNullOrEmpty(x.DoctorFeesUHIAId.ToString()));

            RuleFor(x => x.ItemListPrices).MustAsync(async (Model, ItemListPrices, CancellationToken) =>
            {
                try
                {
                    var doctorUHIA = await DoctorFeesUHIA.Get(Model.DoctorFeesUHIAId, _doctorFeesUHIARepository);

                    foreach (var item in Model.ItemListPrices)
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
           .When(x => x.ItemListPrices != null && x.ItemListPrices.Count() > 0 && _validDrFeesUHIA);

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
          .When(x => x.ItemListPrices != null && x.ItemListPrices.Count() > 0 && _validDrFeesUHIA);
        }
    }
}
