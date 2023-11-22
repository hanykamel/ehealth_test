using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using EHealth.ManageItemLists.Domain.ItemLists;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Commands
{
    public class CreateDevicesAndAssetsUHIABasicDataCommand : CreateDevicesAndAssetsUHIABasicDataDto, IRequest<Guid>
    {
        public CreateDevicesAndAssetsUHIABasicDataCommand(CreateDevicesAndAssetsUHIABasicDataDto request)
        {
            EHealthCode = request.EHealthCode;
            DescriptorAr = request.DescriptorAr;
            DescriptorEn = request.DescriptorEn;
            UnitRoomId = request.UnitRoomId;
            CategoryId = request.CategoryId;
            SubCategoryId = request.SubCategoryId;
            DataEffectiveDateFrom = request.DataEffectiveDateFrom;
            DataEffectiveDateTo = request.DataEffectiveDateTo;
            ItemListId = request.ItemListId;
        }
    }
}
