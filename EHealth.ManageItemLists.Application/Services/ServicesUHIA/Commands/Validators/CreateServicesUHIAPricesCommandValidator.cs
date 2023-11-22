using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.Shared.DTOs;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands.Validators
{
    public class CreateServicesUHIAPricesCommandValidator : AbstractValidator<CreateServicesUHIAPricesCommand>
    {
        private readonly IServiceUHIARepository _serviceUHIARepository;
        private bool _validserviceUHIA = false;
        public CreateServicesUHIAPricesCommandValidator(IServiceUHIARepository serviceUHIARepository) 
        {
            _serviceUHIARepository = serviceUHIARepository;

            RuleFor(x => x.ServiceUHIAId).NotNull().NotEmpty();
            RuleFor(x => x.ItemListPrices).NotNull().NotEmpty();
            RuleFor(x => x.ServiceUHIAId).MustAsync(async (ServiceUHIAId, CancellationToken) =>
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
                .When(x => !string.IsNullOrEmpty(x.ServiceUHIAId.ToString()));

            RuleFor(x => x.ItemListPrices).MustAsync(async(Model, ItemListPrices, CancellationToken) => 
            {
                try
                {
                    var serviceUHIA = await ServiceUHIA.Get(Model.ServiceUHIAId, _serviceUHIARepository);

                    foreach (var item in Model.ItemListPrices)
                    {
                       if( item.EffectiveDateFrom.Date < serviceUHIA.DataEffectiveDateFrom.Date || 
                        (item.EffectiveDateTo.HasValue && serviceUHIA.DataEffectiveDateTo.HasValue && item.EffectiveDateTo.Value.Date > serviceUHIA.DataEffectiveDateTo.Value.Date)||
                        ((!item.EffectiveDateTo.HasValue) && serviceUHIA.DataEffectiveDateTo.HasValue))
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
            .When(x => x.ItemListPrices != null && x.ItemListPrices.Count() > 0  && _validserviceUHIA);

            RuleFor(x => x.ItemListPrices).Must( (Model, CancellationToken) =>
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

                   if(DateAndTimeOperations.DoesNotOverlap(convertedItemLst))
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
           .When(x => x.ItemListPrices != null && x.ItemListPrices.Count() > 0 && _validserviceUHIA);
        }
    }
}
