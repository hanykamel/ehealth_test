using EHealth.ManageItemLists.Domain.ItemListPricing;

namespace EHealth.ManageItemLists.Application.ItemListPrices.DTOs
{
    public class ItemListPriceDto
    {
        public int Id { get; private set; }
        public double Price { get; private set; }

        public string EffectiveDateFrom { get; private set; }
        public string? EffectiveDateTo { get; private set; }
        public bool? IsDeleted { get; set; }

        public static ItemListPriceDto FromItemListPrice(ItemListPrice input) =>
        input is not null ? new ItemListPriceDto
        {
            Id = input.Id,
            Price = input.Price,
            EffectiveDateFrom = input.EffectiveDateFrom.ToString("yyyy-MM-dd"),
            EffectiveDateTo = input.EffectiveDateTo?.ToString("yyyy-MM-dd"),
            IsDeleted = input.IsDeleted
        } : null;
    }
}
