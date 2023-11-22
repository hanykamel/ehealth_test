using EHealth.ManageItemLists.Domain.DrugsPricing;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IDrugPriceRepository
    {
        Task<int> Create(DrugPrice input);
        Task<bool> Update(DrugPrice input);
        Task<bool> Delete(DrugPrice input);
        Task<DrugPrice?> Get(int id);
    }
}
