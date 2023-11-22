using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs
{
    public class UpdateServicesUHIABasicDataDto
    {
        public Guid Id { get; set; }
        public string EHealthCode { get; set; }
        public string UHIAId { get; set; }
        public string? ShortDescAr { get; set; }
        public string? ShortDescEn { get; set; }
        public int ServiceCategoryId { get; set; }
        public int ServiceSubCategoryId { get; set; }
        public int? ItemListId { get; set; }
        public DateTime DataEffectiveDateFrom { get; set; }
        public DateTime? DataEffectiveDateTo { get; set; }

        public ServiceUHIA ToServiceUHIA(string createdBy, string tenantId) => ServiceUHIA.Create(Id, EHealthCode, UHIAId, ShortDescAr, ShortDescEn,
                ServiceCategoryId, ServiceSubCategoryId, (int)ItemListId, DataEffectiveDateFrom, DataEffectiveDateTo, createdBy, tenantId);
    }
}
