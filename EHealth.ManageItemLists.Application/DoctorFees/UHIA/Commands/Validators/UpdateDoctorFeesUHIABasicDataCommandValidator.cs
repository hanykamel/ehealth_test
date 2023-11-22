using EHealth.ManageItemLists.Domain.DoctorFees.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using FluentValidation;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands.Validators
{
    public class UpdateDoctorFeesUHIABasicDataCommandValidator : AbstractValidator<UpdateDoctorFeesUHIABasicDataCommand>
    {
        private readonly IDoctorFeesUHIARepository _doctorFeesUHIARepository;
        private bool _validDoctorFeesUHIA = false;
        public UpdateDoctorFeesUHIABasicDataCommandValidator(IDoctorFeesUHIARepository doctorFeesUHIARepository)
        {
            _doctorFeesUHIARepository = doctorFeesUHIARepository;
            RuleFor(x => x.Id).MustAsync(async (DoctorFeesUHIAId, CancellationToken) =>
            {
                try
                {
                    var doctorFeesUHIA = await DoctorFeesUHIA.Get(DoctorFeesUHIAId, _doctorFeesUHIARepository);
                    if (doctorFeesUHIA is not null)
                    {
                        _validDoctorFeesUHIA = true;
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
            }).WithErrorCode("DoctorFeesUHIANotExist").WithMessage("DoctorFeesUHIA with DoctorFeesUHIAId not exist.")
                .When(x => !string.IsNullOrEmpty(x.Id.ToString()));
            RuleFor(x => new { x.DataEffectiveDateFrom, x.DataEffectiveDateTo }).MustAsync(async (Model, ItemListPrices, CancellationToken) =>
            {
                try
                {
                    var doctorFeesUHIA = await DoctorFeesUHIA.Get(Model.Id, _doctorFeesUHIARepository);
                    var notDeletedItems = doctorFeesUHIA.ItemListPrices.Where(x => x.IsDeleted == false).ToList();
                    foreach (var item in notDeletedItems)
                    {
                        if (item.EffectiveDateFrom.Date < Model.DataEffectiveDateFrom.Date ||
                         (item.EffectiveDateTo.HasValue && Model.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > Model.DataEffectiveDateTo.Value.Date) ||
                         ((!Model.DataEffectiveDateTo.HasValue) && item.EffectiveDateTo.HasValue))
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
          .When(x => _validDoctorFeesUHIA);
        }
    }
}
