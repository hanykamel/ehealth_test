using EHealth.ManageItemLists.Domain.ConsumablesCodesStatus;
using EHealth.ManageItemLists.Domain.Shared.Pagination;


namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IConsumablesCodeStatusRepository
    {
        Task<int> CreateConsumablesCodeStatus(ConsumablesCodeStatus input);
        Task<bool> UpdateConsumablesCodeStatus(ConsumablesCodeStatus input);
        Task<bool> DeleteConsumablesCodeStatus(ConsumablesCodeStatus input);
        Task<ConsumablesCodeStatus?> Get(int id);
        Task<PagedResponse<ConsumablesCodeStatus>> Search(int Id, string? Code, string? CodeStatusDescAr, string? CodeStatusDescEng, bool Active, int pageNumber, int pageSize);
    }
}
