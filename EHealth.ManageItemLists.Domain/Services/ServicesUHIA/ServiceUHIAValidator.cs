using FluentValidation;

namespace EHealth.ManageItemLists.Domain.Services.ServicesUHIA
{
    public class ServiceUHIAValidator : AbstractValidator<ServiceUHIA>
    {
        public ServiceUHIAValidator()
        {

            RuleFor(x => x.EHealthCode).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.UHIAId).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty().MinimumLength(1).MaximumLength(501);
            RuleFor(x => x.ShortDescAr).Cascade(CascadeMode.StopOnFirstFailure).Length(5, 200).When(x => !string.IsNullOrEmpty(x.ShortDescAr));
            RuleFor(x => x.ShortDescEn).Cascade(CascadeMode.StopOnFirstFailure).Length(5, 200).When(x => !string.IsNullOrEmpty(x.ShortDescEn));
            RuleFor(x => x.ServiceCategoryId).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty();
            RuleFor(x => x.ServiceSubCategoryId).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty();
            RuleFor(x => x.ItemListId).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty();
            RuleFor(x => x.DataEffectiveDateFrom).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty();
            RuleFor(x => x.DataEffectiveDateTo).Cascade(CascadeMode.StopOnFirstFailure).Must((model, EffectiveDateTo) =>
            {
                if (model.DataEffectiveDateFrom < EffectiveDateTo.Value) { return true; }
                else return false;
            }).When(x => x.DataEffectiveDateTo.HasValue);
        }
    }
}
