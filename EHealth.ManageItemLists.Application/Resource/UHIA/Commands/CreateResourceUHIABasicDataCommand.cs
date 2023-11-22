using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Resource.UHIA.DTOs;
using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.Sub_Categories;
using EHealth.ManageItemLists.Domain.UnitRooms;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Resource.UHIA.Commands
{
    public class CreateResourceUHIABasicDataCommand : CreateResourceUHIABasicDataDto, IRequest<Guid>
    {
        public CreateResourceUHIABasicDataCommand(CreateResourceUHIABasicDataDto request)
        {
            EHealthCode = request.EHealthCode;
            DescriptorAr = request.DescriptorAr;
            DescriptorEn = request.DescriptorEn;
            CategoryId = request.CategoryId;
            SubCategoryId = request.SubCategoryId;
            DataEffectiveDateFrom = request.DataEffectiveDateFrom;
            DataEffectiveDateTo = request.DataEffectiveDateTo;
            ItemListId = request.ItemListId;
        }
    }
}
