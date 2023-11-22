using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.UnitRooms.DTOs;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.DTOs
{
    public class CreateDevicesAndAssetsUHIABasicDataDto
    {
        public string EHealthCode { get; set; }
        public string DescriptorAr { get; set; }
        public string DescriptorEn { get; set; }
        public int? UnitRoomId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ItemListId { get; set; }
        public DateTime DataEffectiveDateFrom { get; set; }
        public DateTime? DataEffectiveDateTo { get; set; }

        public DevicesAndAssetsUHIA ToDevicesAndAssetsUHIA(string createdBy, string tenantId) => DevicesAndAssetsUHIA.Create(null,EHealthCode, DescriptorAr, DescriptorEn, UnitRoomId
                , CategoryId, SubCategoryId, DataEffectiveDateFrom, DataEffectiveDateTo, ItemListId, createdBy, tenantId);

    }
}
