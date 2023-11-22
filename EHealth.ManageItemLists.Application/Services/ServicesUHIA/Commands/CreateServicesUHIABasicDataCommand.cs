using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands
{
    public class CreateServicesUHIABasicDataCommand : CreateServicesUHIABasicDataDto, IRequest<Guid>
    {
        public CreateServicesUHIABasicDataCommand(CreateServicesUHIABasicDataDto request) 
        {
            EHealthCode = request.EHealthCode;
            UhiaId = request.UhiaId;
            ShortDescAr = request.ShortDescAr;
            ShortDescEn = request.ShortDescEn;
            ServiceCategoryId = request.ServiceCategoryId;
            ServiceSubCategoryId = request.ServiceSubCategoryId;
            ItemListId = request.ItemListId;
            DataEffectiveDateFrom = request.DataEffectiveDateFrom;
            DataEffectiveDateTo = request.DataEffectiveDateTo;
            
        }
    }
}
