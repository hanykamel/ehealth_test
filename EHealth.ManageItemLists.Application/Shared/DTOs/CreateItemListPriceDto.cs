using EHealth.ManageItemLists.Domain.ConsumablesAndDevices;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Shared.DTOs
{
    public class CreateItemListPriceDto
    {
        public double Price { get; set; }
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime? EffectiveDateTo { get; set; }
        public ItemListPrice ToItemListPrice(string createdBy, string tenantId) => ItemListPrice.Create(null, Price, EffectiveDateFrom, EffectiveDateTo, createdBy, tenantId);
    }
}
