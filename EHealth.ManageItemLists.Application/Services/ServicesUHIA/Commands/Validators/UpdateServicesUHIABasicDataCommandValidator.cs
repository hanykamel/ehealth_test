using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands.Validators
{
    public class UpdateServicesUHIABasicDataCommandValidator : AbstractValidator<UpdateServicesUHIABasicDataCommand>
    {
        private readonly IServiceUHIARepository _serviceUHIARepository;
        private bool _validserviceUHIA = false;
        public UpdateServicesUHIABasicDataCommandValidator(IServiceUHIARepository serviceUHIARepository)
        {
            _serviceUHIARepository = serviceUHIARepository;

            RuleFor(x => x.Id).MustAsync(async (ServiceUHIAId, CancellationToken) =>
            {
                try
                {
                    var serviceUHIA = await ServiceUHIA.Get(ServiceUHIAId, _serviceUHIARepository);
                    if (serviceUHIA is not null)
                    {
                        _validserviceUHIA = true;
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
            }).WithErrorCode("ServiceUHIANotExist").WithMessage("ServiceUHIA with ServiceUHIAId not exist.")
                .When(x => !string.IsNullOrEmpty(x.Id.ToString()));

            RuleFor(x => new {x.DataEffectiveDateFrom, x.DataEffectiveDateTo}).MustAsync(async (Model, ItemListPrices, CancellationToken) =>
            {
                try
                {
                    var serviceUHIA = await ServiceUHIA.Get(Model.Id, _serviceUHIARepository);
                    var notDeletedItems = serviceUHIA.ItemListPrices.Where(x => x.IsDeleted == false).ToList();
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
            .When(x => _validserviceUHIA);
        }
    }
}
