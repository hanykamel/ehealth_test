using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.WorkingHours;


namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IWorkingHourRepository
    {
        Task<int> Create(WorkingHour input);
        Task<bool> Update(WorkingHour input);
        Task<bool> Delete(WorkingHour input);
        Task<WorkingHour?> Get(int id);
        Task<PagedResponse<WorkingHour>> Search(int id,TimeOnly fromTime, TimeOnly toTime, DayOfWeek nonWorkingDays, int pageNumber, int pageSize);
    }
}
