using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
using FluentValidation;


namespace EHealth.ManageItemLists.Domain.LocalUnitOfMeasure
{
    public class UnitOfMeasureValidator : AbstractValidator<UnitOfMeasure>
    {
        public UnitOfMeasureValidator() 
        {
            RuleFor(x => x.MeasureTypeAr).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.MeasureTypeENG).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.DefinitionAr).MinimumLength(1).MaximumLength(1500);
            RuleFor(x => x.DefinitionENG).MinimumLength(1).MaximumLength(1500);
        }
       
    }
}
