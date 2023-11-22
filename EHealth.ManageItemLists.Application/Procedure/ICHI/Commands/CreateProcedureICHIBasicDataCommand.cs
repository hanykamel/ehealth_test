using EHealth.ManageItemLists.Application.Procedure.ICHI.DTOs;
using EHealth.ManageItemLists.Domain.ItemLists;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Procedure.ICHI.Commands
{
    public class CreateProcedureICHIBasicDataCommand : CreateProcedureICHIBasicDataDto, IRequest<Guid>
    {
        public CreateProcedureICHIBasicDataCommand(CreateProcedureICHIBasicDataDto request)
        {
            EHealthCode = request.EHealthCode;
            UHIAId = request.UHIAId;
            TitleAr = request.TitleAr;
            TitleEn = request.TitleEn;
            ServiceCategoryId = request.ServiceCategoryId;
            ServiceSubCategoryId = request.ServiceSubCategoryId;
            ItemListId = request.ItemListId;
            DataEffectiveDateFrom = request.DataEffectiveDateFrom;
            DataEffectiveDateTo = request.DataEffectiveDateTo;
            LocalSpecialtyDepartmentId = request.LocalSpecialtyDepartmentId;
        }
    }
}
