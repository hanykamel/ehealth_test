using EHealth.ManageItemLists.Domain.NotAssignedStatusConfigurationScreens;
using EHealth.ManageItemLists.Domain.Shared.Pagination;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface INotAssignedStatusConfigurationScreenRepository
    {
        Task<int> Create(NotAssignedStatusConfigurationScreen input);
        Task<bool> Update(NotAssignedStatusConfigurationScreen input);
        Task<bool> Delete(NotAssignedStatusConfigurationScreen input);
        Task<NotAssignedStatusConfigurationScreen?> Get(int id);
        Task<PagedResponse<NotAssignedStatusConfigurationScreen>> Search(int Id, int TotalNumberOfDays, int SendNotificationEvery, int pageNumber, int pageSize);
    }
}
