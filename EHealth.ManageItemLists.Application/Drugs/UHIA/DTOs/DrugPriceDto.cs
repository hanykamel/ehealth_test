using EHealth.ManageItemLists.Application.Lookups.PriceUnits.DTOs;
using EHealth.ManageItemLists.Application.Resource.ItemPrice.DTOs;
using EHealth.ManageItemLists.Domain.DrugsPricing;
using EHealth.ManageItemLists.Domain.Resource.ItemPrice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.DTOs
{
    public class DrugPriceDto
    {
        public int Id { get; private set; }
        public double MainUnitPrice { get; private set; }
        public double FullPackPrice { get; private set; }
        public double SubUnitPrice { get; private set; }

        public string EffectiveDateFrom { get; private set; }
        public string? EffectiveDateTo { get; private set; }
        public bool? IsDeleted { get; set; }


        public static DrugPriceDto FromDrugPrice(DrugPrice input) =>
        input is not null ? new DrugPriceDto
        {
            Id = input.Id,
            MainUnitPrice = input.MainUnitPrice,
            EffectiveDateFrom = input.EffectiveDateFrom.ToString("yyyy-MM-dd"),
            EffectiveDateTo = input.EffectiveDateTo?.ToString("yyyy-MM-dd"),
            SubUnitPrice = input.SubUnitPrice,
            FullPackPrice = input.FullPackPrice,
            IsDeleted = input.IsDeleted
        } : null;
    }
}
