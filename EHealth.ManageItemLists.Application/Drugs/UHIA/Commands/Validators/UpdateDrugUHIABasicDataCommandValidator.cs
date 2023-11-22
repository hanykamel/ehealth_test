using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
using EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI;
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
    public class UpdateDrugUHIABasicDataCommandValidator : AbstractValidator<UpdateDrugUHIABasicDataCommand>
    {
        private readonly IDrugsUHIARepository _drugsUHIARepository;
        private bool _valid = false;
        public UpdateDrugUHIABasicDataCommandValidator(IDrugsUHIARepository drugsUHIARepository)
        {
            _drugsUHIARepository = drugsUHIARepository;
            RuleFor(x => x.Id).MustAsync(async (Id, CancellationToken) =>
            {
                try
                {
                    var drugUHIA = await DrugUHIA.Get(Id, _drugsUHIARepository);
                    if (drugUHIA is not null)
                    {
                        _valid = true;
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

            RuleFor(x => new { x.DataEffectiveDateFrom, x.DataEffectiveDateTo }).MustAsync(async (Model, ItemListPrices, CancellationToken) =>
            {
                try
                {
                    var drugUHIA = await DrugUHIA.Get(Model.Id, _drugsUHIARepository);
                    var notDeletedItems = drugUHIA.DrugPrices.Where(x => x.IsDeleted == false).ToList();
                    foreach (var item in notDeletedItems)
                    {
                        if (item.EffectiveDateFrom.Date < Model.DataEffectiveDateFrom.Date ||
                         (item.EffectiveDateTo.HasValue && Model.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > Model.DataEffectiveDateTo.Value.Date) ||
                         ((!item.EffectiveDateTo.HasValue) && Model.DataEffectiveDateTo.HasValue))
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
            .When(x => _valid);

        }
       
}
}
