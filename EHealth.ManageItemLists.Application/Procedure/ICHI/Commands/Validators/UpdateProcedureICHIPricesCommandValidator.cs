using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.Shared.DTOs;
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
    public class UpdateProcedureICHIPricesCommandValidator : AbstractValidator<UpdateProcedureICHIPricesCommand>
    {
        private readonly IProcedureICHIRepository _procedureICHIRepository;
        private bool _valid = false;
        public UpdateProcedureICHIPricesCommandValidator(IProcedureICHIRepository procedureICHIRepository)
        {
            _procedureICHIRepository = procedureICHIRepository;

            RuleFor(x => x.ProcedureICHIId).NotNull().NotEmpty();
            RuleFor(x => x.ItemListPrices).NotNull().NotEmpty();
            RuleFor(x => x.ProcedureICHIId).MustAsync(async (Id, CancellationToken) =>
            {
                try
                {
                    var rocedureICHI = await ProcedureICHI.Get(Id, _procedureICHIRepository);
                    if (rocedureICHI is not null)
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
                .When(x => !string.IsNullOrEmpty(x.ProcedureICHIId.ToString()));

            RuleFor(x => x.ItemListPrices).MustAsync(async (Model, ItemListPrices, CancellationToken) =>
            {
                try
                {
                    var rocedureICHI = await ProcedureICHI.Get(Model.ProcedureICHIId, _procedureICHIRepository);

                    foreach (var item in Model.ItemListPrices)
                    {
                        if (item.EffectiveDateFrom.Date < rocedureICHI.DataEffectiveDateFrom.Date ||
                         (item.EffectiveDateTo.HasValue && rocedureICHI.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > rocedureICHI.DataEffectiveDateTo.Value.Date) ||
                         ((!item.EffectiveDateTo.HasValue) && rocedureICHI.DataEffectiveDateTo.HasValue))
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
            .When(x => x.ItemListPrices != null && x.ItemListPrices.Count() > 0 && _valid);

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
           .When(x => x.ItemListPrices != null && x.ItemListPrices.Count() > 0 && _valid); ;
        }
    }
}
