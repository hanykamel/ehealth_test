using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHealth.ManageItemLists.Domain.ConsumablesAndDevices;
using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs
{
    public class CreateConsAndDevUHIABasicDataDto
    {
        public string EHealthCode { get; set; }
        public string UHIAId { get; set; }
        public string? ShortDescAr { get; set; }
        public string ShortDescEn { get; set; }
        public int ServiceCategoryId { get; set; }
        public int ServiceSubCategoryId { get; set; }
        public int ItemListId { get; set; }
        public DateTime DataEffectiveDateFrom { get; set; }
        public DateTime? DataEffectiveDateTo { get; set; }
        public int UnitOfMeasureId { get; set; }

        public Domain.ConsumablesAndDevices.ConsumablesAndDevicesUHIA ToConsumablesAndDevicesUHIA(string createdBy, string tenantId) => Domain.ConsumablesAndDevices.ConsumablesAndDevicesUHIA.Create(null, EHealthCode, UHIAId, ShortDescAr, ShortDescEn,
                 ServiceCategoryId, ServiceSubCategoryId, ItemListId, UnitOfMeasureId, DataEffectiveDateFrom, DataEffectiveDateTo, createdBy, tenantId);
    }
}
