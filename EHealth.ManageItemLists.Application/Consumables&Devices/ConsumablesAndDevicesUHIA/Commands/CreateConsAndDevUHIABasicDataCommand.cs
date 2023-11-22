using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Commands
{
    public class CreateConsAndDevUHIABasicDataCommand : CreateConsAndDevUHIABasicDataDto, IRequest<Guid>
    {
        public CreateConsAndDevUHIABasicDataCommand(CreateConsAndDevUHIABasicDataDto request)
        {
            EHealthCode = request.EHealthCode;
            UHIAId = request.UHIAId;
            ShortDescAr = request.ShortDescAr;
            ShortDescEn = request.ShortDescEn;
            ServiceCategoryId = request.ServiceCategoryId;
            ServiceSubCategoryId = request.ServiceSubCategoryId;
            ItemListId = request.ItemListId;
            DataEffectiveDateFrom = request.DataEffectiveDateFrom;
            DataEffectiveDateTo = request.DataEffectiveDateTo;
            UnitOfMeasureId = request.UnitOfMeasureId;

        }
    }
}
