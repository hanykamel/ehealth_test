using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands.Validators;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands
{
    public class UpdateServicesUHIABasicDataCommand: UpdateServicesUHIABasicDataDto, IRequest<bool>, IValidationModel<UpdateServicesUHIABasicDataCommand>
    {
        private readonly IServiceUHIARepository _serviceUHIARepository;
        public UpdateServicesUHIABasicDataCommand(UpdateServicesUHIABasicDataDto request, IServiceUHIARepository serviceUHIARepository)
        {
            Id = request.Id;
            EHealthCode = request.EHealthCode;
            UHIAId = request.UHIAId;
            ShortDescAr = request.ShortDescAr;
            ShortDescEn = request.ShortDescEn;
            ServiceCategoryId = request.ServiceCategoryId;
            ServiceSubCategoryId = request.ServiceSubCategoryId;
            //ItemListId = request.ItemListId;
            DataEffectiveDateFrom = request.DataEffectiveDateFrom;
            DataEffectiveDateTo = request.DataEffectiveDateTo;
            _serviceUHIARepository = serviceUHIARepository;
        }
        public AbstractValidator<UpdateServicesUHIABasicDataCommand> Validator => new UpdateServicesUHIABasicDataCommandValidator(_serviceUHIARepository);
    }
}
