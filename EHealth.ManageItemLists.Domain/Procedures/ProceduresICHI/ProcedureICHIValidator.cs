using FluentValidation;

namespace EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI
{
    public class ProcedureICHIValidator : AbstractValidator<ProcedureICHI>
    {
        public ProcedureICHIValidator()
        {
            RuleFor(x => x.EHealthCode).NotNull().NotEmpty().MinimumLength(1).MaximumLength(500);
            RuleFor(x => x.UHIAId).NotNull().NotEmpty().MinimumLength(1).MaximumLength(500);
            RuleFor(x => x.TitleEn).NotNull().NotEmpty().MinimumLength(2).MaximumLength(250);
            RuleFor(x => x.TitleAr).Length(2, 250).When(x => !string.IsNullOrEmpty(x.TitleAr));
            RuleFor(x => x.ServiceCategoryId).NotNull().NotEmpty();
            RuleFor(x => x.SubCategoryId).NotNull().NotEmpty();
            RuleFor(x => x.ItemListId).NotNull().NotEmpty();
            //RuleFor(x => x.LocalSpecialtyDepartmentId).NotNull().NotEmpty();
            RuleFor(x => x.DataEffectiveDateTo).Must((model, EffectiveDateTo) =>
            {
                if (model.DataEffectiveDateFrom < EffectiveDateTo.Value) { return true; }
                else return false;
            }).When(x => x.DataEffectiveDateTo.HasValue);


        }
    }
}
