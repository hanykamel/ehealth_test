using FluentValidation;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace EHealth.ManageItemLists.Domain.ItemListPricing
{
    public class ItemListPriceValidator : AbstractValidator<ItemListPrice>
    {
        public ItemListPriceValidator()
        {
            RuleFor(x => x.Price).NotEmpty().NotNull();

            RuleFor(x => x.Price).Must((Price) =>
            {
                String regex = "^(?![0.]*$)\\d{1,7}(?:\\.\\d{1,4})?$";
                string priceString = Price.ToString();
                if (Price >0 && Regex.IsMatch(priceString, regex))
                {
                    return true;
                }
                else return false;

            }).WithErrorCode("InvalidPrice").WithMessage("Only accepts positive values from 1 to 7 digits before the decimal point and 2 to 4 decimal places.");

            RuleFor(x => x.EffectiveDateFrom).NotEmpty().NotNull();
            RuleFor(x => x.EffectiveDateTo).Must((model, EffectiveDateTo) =>
            {
                if (model.EffectiveDateFrom < EffectiveDateTo.Value) { return true; }
                else return false;
            }).When(x => x.EffectiveDateTo.HasValue);

        }
    }
}
