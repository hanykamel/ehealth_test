using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
using EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Procedure.ICHI.Commands.Validators
{
    public class UpdateProcedureICHIBasicDataCommandValidator : AbstractValidator<UpdateProcedureICHIBasicDataCommand>
    {
        private readonly IProcedureICHIRepository _procedureICHIRepository;
        private bool _valid = false;
        public UpdateProcedureICHIBasicDataCommandValidator(IProcedureICHIRepository procedureICHIRepository)
        {
            _procedureICHIRepository = procedureICHIRepository;

            RuleFor(x => x.Id).MustAsync(async (Id, CancellationToken) =>
            {
                try
                {
                    var procedureICHI = await ProcedureICHI.Get(Id, _procedureICHIRepository);
                    if (procedureICHI is not null)
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
            }).WithErrorCode("ProcedureICHINotExist").WithMessage("ProcedureICHI with Id not exist.")
                .When(x => !string.IsNullOrEmpty(x.Id.ToString()));

            RuleFor(x => new { x.DataEffectiveDateFrom, x.DataEffectiveDateTo }).MustAsync(async (Model, ItemListPrices, CancellationToken) =>
            {
                try
                {
                    var procedureICHI = await ProcedureICHI.Get(Model.Id, _procedureICHIRepository);
                    var notDeletedItems = procedureICHI.ItemListPrices.Where(x => x.IsDeleted == false).ToList();
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
