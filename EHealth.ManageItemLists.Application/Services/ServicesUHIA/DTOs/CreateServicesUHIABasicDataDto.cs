using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Sub_Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs
{
    public class CreateServicesUHIABasicDataDto
    {
        public string EHealthCode { get;  set; }
        public string UhiaId { get; set; }
        public string? ShortDescAr { get; set; }
        public string? ShortDescEn { get; set; }
        public int ServiceCategoryId { get; set; }
        public int ServiceSubCategoryId { get; set; }
        public int ItemListId { get; set; }
        public DateTime DataEffectiveDateFrom { get; set; }
        public DateTime? DataEffectiveDateTo { get; set; }

        public ServiceUHIA ToServiceUHIA(string createdBy, string tenantId) => ServiceUHIA.Create(null, EHealthCode, UhiaId, ShortDescAr, ShortDescEn,
                 ServiceCategoryId, ServiceSubCategoryId, ItemListId, DataEffectiveDateFrom, DataEffectiveDateTo, createdBy, tenantId);
    }
}
