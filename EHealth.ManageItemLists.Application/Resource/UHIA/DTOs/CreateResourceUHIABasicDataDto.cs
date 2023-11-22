using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.DTOs;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
using EHealth.ManageItemLists.Domain.UnitRooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Resource.UHIA.DTOs
{
    public class CreateResourceUHIABasicDataDto
    {
        public string EHealthCode { get; set; }
        public string? DescriptorAr { get; set; }
        public string DescriptorEn { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ItemListId { get; set; }
        public DateTime DataEffectiveDateFrom { get; set; }
        public DateTime? DataEffectiveDateTo { get; set; }

        public ResourceUHIA ToResourceUHIA(string createdBy, string tenantId) => ResourceUHIA.Create(null,EHealthCode, DescriptorAr, DescriptorEn
                , CategoryId, SubCategoryId, DataEffectiveDateFrom, ItemListId, DataEffectiveDateTo, createdBy, tenantId);
    }
}
