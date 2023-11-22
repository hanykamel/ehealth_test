using EHealth.ManageItemLists.Application.Lookups.PriceUnits.DTOs;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.PriceUnits;
using EHealth.ManageItemLists.Domain.Resource.ItemPrice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Resource.ItemPrice.DTOs
{
    public class ResourceItemPriceDto
    {
        public int Id { get; private set; }
        public double Price { get; private set; }
        public PriceUnitDto PriceUnit { get; private set; }
        public int PriceUnitId { get; private set; }
        public string EffectiveDateFrom { get; private set; }
        public string? EffectiveDateTo { get; private set; }
        public bool? IsDeleted { get; set; }
        public static ResourceItemPriceDto FromResourceItemPrice(ResourceItemPrice input) =>
        input is not null ? new ResourceItemPriceDto
        {
            Id = input.Id,
            Price = input.Price,
            EffectiveDateFrom = input.EffectiveDateFrom.ToString("yyyy-MM-dd"),
            EffectiveDateTo = input.EffectiveDateTo?.ToString("yyyy-MM-dd"),
            PriceUnitId = input.PriceUnitId,
            PriceUnit = PriceUnitDto.FromPriceUnit(input.PriceUnit),
            IsDeleted = input.IsDeleted
    } : null;
    }
}
