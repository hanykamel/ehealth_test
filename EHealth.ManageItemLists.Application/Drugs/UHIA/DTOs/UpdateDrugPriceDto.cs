using EHealth.ManageItemLists.Domain.DrugsPricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.DTOs
{
    public class UpdateDrugPriceDto
    {
        public int Id { get; set; }
        public double MainUnitPrice { get; set; }
        public double FullPackPrice { get; set; }
        public double SubUnitPrice { get; set; }
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime? EffectiveDateTo { get; set; }

        public DrugPrice ToDrugPrice(string createdBy, string tenantId) => DrugPrice.Create(null, MainUnitPrice, FullPackPrice, SubUnitPrice, EffectiveDateFrom, EffectiveDateTo, createdBy, tenantId);

    }
}
