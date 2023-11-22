using FluentValidation;
namespace EHealth.ManageItemLists.Domain.LocalSpecialtyDepartments
{
    public class LocalSpecialtyDepartmentValidator : AbstractValidator<LocalSpecialtyDepartment>
    {
        public LocalSpecialtyDepartmentValidator()
        {
            RuleFor(x => x.LocalSpecialityAr).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.LocalSpecialityENG).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.DefinitionAr).MinimumLength(1).MaximumLength(1500);
            RuleFor(x => x.DefinitionENG).MinimumLength(1).MaximumLength(1500);
        }
    }
}
