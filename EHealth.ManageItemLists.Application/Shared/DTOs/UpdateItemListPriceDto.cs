using EHealth.ManageItemLists.Domain.ItemListPricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Shared.DTOs
{
    public class UpdateItemListPriceDto
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime? EffectiveDateTo { get; set; }
        //public bool IsDeleted { get; set; }
        public ItemListPrice ToItemListPrice(string createdBy, string tenantId) => ItemListPrice.Create(Id, Price, EffectiveDateFrom, EffectiveDateTo, createdBy, tenantId);
    }
}
