using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Commands.Validators;
using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands.Validators;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using EHealth.ManageItemLists.Domain.Sub_Categories;
using EHealth.ManageItemLists.Domain.UnitRooms;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Commands
{
    public class UpdateDevicesAndAssetsUHIABasicDataCommand : UpdateDevicesAndAssetsUHIABasicDataDto, IRequest<bool>, IValidationModel<UpdateDevicesAndAssetsUHIABasicDataCommand>
    {
        private readonly IDevicesAndAssetsUHIARepository _devicesAndAssetsUHIARepository;
        public UpdateDevicesAndAssetsUHIABasicDataCommand(UpdateDevicesAndAssetsUHIABasicDataDto request, IDevicesAndAssetsUHIARepository devicesAndAssetsUHIARepository)
        {
            Id = request.Id;
            EHealthCode = request.EHealthCode;
            DescriptorAr = request.DescriptorAr;
            DescriptorEn = request.DescriptorEn;
            CategoryId = request.CategoryId;
            SubCategoryId = request.SubCategoryId;
            UnitRoomId = request.UnitRoomId;
            //ItemListId = request.ItemListId;
            DataEffectiveDateFrom = request.DataEffectiveDateFrom;
            DataEffectiveDateTo = request.DataEffectiveDateTo;
            _devicesAndAssetsUHIARepository = devicesAndAssetsUHIARepository;
        }
        public AbstractValidator<UpdateDevicesAndAssetsUHIABasicDataCommand> Validator => new UpdateDevicesAndAssetsUHIABasicDataCommandValidator(_devicesAndAssetsUHIARepository);
    }
}
