using EHealth.ManageItemLists.Domain.ItemListPricing;
using System.Collections.Generic;

namespace EHealth.ManageItemLists.Application.Lookups.ItemListPrices.DTOs
{
    public class ItemListPriceDto
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public string EffectiveDateFrom { get; set; }
        public string? EffectiveDateTo { get; set; }
        public bool IsDeleted { get; set; }


        public static IList<ItemListPriceDto> FromItemPrice(IList<ItemListPrice> input)
        {
            IList<ItemListPriceDto> itemPriceList = new List<ItemListPriceDto>();
            foreach (var item in input)
            {
                itemPriceList.Add(new ItemListPriceDto
                {
                    Id = item.Id,
                    Price = item.Price,
                    EffectiveDateFrom = item.EffectiveDateFrom.ToString("yyyy-MM-dd"),
                    EffectiveDateTo = item.EffectiveDateTo?.ToString("yyyy-MM-dd"),
                    IsDeleted = item.IsDeleted
                });
            }
            return itemPriceList;

        }

    }
}
