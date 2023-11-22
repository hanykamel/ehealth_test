using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.Resource.ItemPrice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Resource.ItemPrice.DTOs
{
    public class UpdateResourceItemPriceDto
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int PriceUnitId { get; set; }
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime? EffectiveDateTo { get; set; }
        public ResourceItemPrice ToResourceItemPrice(string createdBy, string tenantId) => ResourceItemPrice.Create(Id, Price, PriceUnitId, EffectiveDateFrom, EffectiveDateTo, createdBy, tenantId);
    }
}
