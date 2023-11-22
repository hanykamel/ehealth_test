using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.DTOs
{
    public class UpdateDevicesAndAssetsUHIABasicDataDto
    {
        public Guid Id { get; set; }
        public string EHealthCode { get; set; }
        public string DescriptorAr { get; set; }
        public string DescriptorEn { get; set; }
        public int? UnitRoomId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int? ItemListId { get; set; }
        public DateTime DataEffectiveDateFrom { get; set; }
        public DateTime? DataEffectiveDateTo { get; set; }
        public DevicesAndAssetsUHIA ToDevAndAssetsUHIA(string createdBy, string tenantId) => DevicesAndAssetsUHIA.Create(Id,EHealthCode, DescriptorAr, DescriptorEn, UnitRoomId,
            CategoryId, SubCategoryId, DataEffectiveDateFrom, DataEffectiveDateTo, (int)ItemListId, createdBy, tenantId);
    }
}
