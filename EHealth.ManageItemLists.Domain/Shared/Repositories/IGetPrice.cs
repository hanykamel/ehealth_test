using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.Resource.ItemPrice;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IGetPrice
    {
        Task<ResourceItemPrice?> GetPriceByDate(DateTime? date);
    }
}
