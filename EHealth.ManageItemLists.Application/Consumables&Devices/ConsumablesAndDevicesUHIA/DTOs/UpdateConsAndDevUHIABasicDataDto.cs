using EHealth.ManageItemLists.DataAccess.Migrations;
using EHealth.ManageItemLists.Domain.ConsumablesAndDevices;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs
{
    public class UpdateConsAndDevUHIABasicDataDto
    {
        public Guid Id { get; set; }
        public string EHealthCode { get; set; }
        public string UHIAId { get; set; }
        public string? ShortDescAr { get; set; }
        public string ShortDescEn { get; set; }
        public int ServiceCategoryId { get; set; }
        public int ServiceSubCategoryId { get; set; }
        public int? ItemListId { get; set; }
        public DateTime DataEffectiveDateFrom { get; set; }
        public DateTime? DataEffectiveDateTo { get; set; }
        public int UnitOfMeasureId { get; set; }
        public int LocalUnitOfMeasureId { get; set; }

        public Domain.ConsumablesAndDevices.ConsumablesAndDevicesUHIA ToConsAndDevUHIA(string createdBy, string tenantId) => Domain.ConsumablesAndDevices.ConsumablesAndDevicesUHIA.Create(Id, EHealthCode, UHIAId, ShortDescAr, ShortDescEn,
                ServiceCategoryId, ServiceSubCategoryId, (int)ItemListId,LocalUnitOfMeasureId, DataEffectiveDateFrom, DataEffectiveDateTo, createdBy, tenantId);
    }
}
    