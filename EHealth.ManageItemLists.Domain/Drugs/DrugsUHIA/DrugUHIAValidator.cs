using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA
{
    public class DrugUHIAValidator:AbstractValidator<DrugUHIA>
    {
        public DrugUHIAValidator()
        {
            RuleFor(x => x.EHealthDrugCode).MinimumLength(2).MaximumLength(60);
            RuleFor(x => x.LocalDrugCode).NotNull().NotEmpty().MinimumLength(2).MaximumLength(60);
            RuleFor(x => x.ProprietaryName).NotNull().NotEmpty().MinimumLength(4).MaximumLength(270);
            RuleFor(x => x.DosageForm).NotNull().NotEmpty().MinimumLength(3).MaximumLength(280);
            //RuleFor(x => x.RouteOfAdministration).MinimumLength(2).MaximumLength(150);
            //RuleFor(x => x.Manufacturer).MinimumLength(4).MaximumLength(270);
            //RuleFor(x => x.MarketAuthorizationHolder).MinimumLength(4).MaximumLength(270);
            //RuleFor(x => x.NumberOfMainUnit).GreaterThanOrEqualTo(1).LessThanOrEqualTo(1000);
            RuleFor(x => x.SubUnitId).NotNull().NotEmpty();
            RuleFor(x => x.ItemListId).NotNull().NotEmpty();
            //RuleFor(x => x.NumberOfSubunitPerMainUnit).GreaterThanOrEqualTo(1).LessThanOrEqualTo(20);
            //RuleFor(x => x.TotalNumberSubunitsOfPack).GreaterThanOrEqualTo(1).LessThanOrEqualTo(20);
            RuleFor(x => x.DataEffectiveDateFrom).NotNull().NotEmpty();
            RuleFor(x => x.DataEffectiveDateTo).Must((model, EffectiveDateTo) =>
            {
                if (model.DataEffectiveDateFrom < EffectiveDateTo.Value) { return true; }
                else return false;
            }).When(x => x.DataEffectiveDateTo.HasValue);
        }
    }
}
