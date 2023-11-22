
using EHealth.ManageItemLists.Domain.Pre_authorizationProtocol;
using EHealth.ManageItemLists.Domain.ReimbursementCategories;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IReimbursementCategoryRepository
    {
        Task<int> CreateReimbursementCategory(ReimbursementCategory input);
        Task<bool> UpdateReimbursementCategory(ReimbursementCategory input);
        Task<bool> DeleteReimbursementCategory(ReimbursementCategory input);
        Task<ReimbursementCategory?> Get(int id);
        Task<PagedResponse<ReimbursementCategory>> Search(Expression<Func<ReimbursementCategory, bool>> predicate, int pageNumber, int pageSize, bool enablePagination);
    }
}
