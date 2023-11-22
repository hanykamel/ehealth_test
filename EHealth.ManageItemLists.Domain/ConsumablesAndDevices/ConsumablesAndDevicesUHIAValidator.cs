using FluentValidation;

namespace EHealth.ManageItemLists.Domain.ConsumablesAndDevices
{
    public class ConsumablesAndDevicesUHIAValidator : AbstractValidator<ConsumablesAndDevicesUHIA>
    {
        public ConsumablesAndDevicesUHIAValidator()
        {
            RuleFor(x => x.EHealthCode).NotNull().NotEmpty().MinimumLength(1).MaximumLength(500);
            RuleFor(x => x.UHIAId).NotNull().NotEmpty().MinimumLength(1).MaximumLength(500);
            RuleFor(x => x.ShortDescriptorAr).Length(4,60).When(x => !string.IsNullOrEmpty(x.ShortDescriptorAr));
            RuleFor(x => x.ShortDescriptorEn).NotNull().NotEmpty().MinimumLength(4).MaximumLength(60);
            RuleFor(x => x.UnitOfMeasureId).NotNull().NotEmpty();
            RuleFor(x => x.ItemListId).NotNull().NotEmpty();
            RuleFor(x => x.ServiceCategoryId).NotNull().NotEmpty();
            RuleFor(x => x.SubCategoryId).NotNull().NotEmpty();
            RuleFor(x => x.DataEffectiveDateFrom).NotNull().NotEmpty();
            RuleFor(x => x.DataEffectiveDateTo).Must((model, DataEffectiveDateTo) =>
            {
                if (model.DataEffectiveDateFrom < DataEffectiveDateTo.Value) return true;
                else return false;
            }).When(x => x.DataEffectiveDateTo.HasValue);
        }
    }
}
