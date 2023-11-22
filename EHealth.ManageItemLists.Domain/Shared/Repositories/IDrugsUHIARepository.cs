using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IDrugsUHIARepository
    {
        Task<Guid> Create(DrugUHIA input);
        Task<bool> Update(DrugUHIA input);
        Task<bool> Delete(DrugUHIA input);
        Task<DrugUHIA?> Get(Guid id);
        Task<PagedResponse<DrugUHIA>> Search(Expression<Func<DrugUHIA, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending);
        Task<bool> IsItemLIstBusy(int itemListId);
    }
}
