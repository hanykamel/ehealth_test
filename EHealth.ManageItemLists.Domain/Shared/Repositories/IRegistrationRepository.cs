using EHealth.ManageItemLists.Domain.RegistrationTypes;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IRegistrationRepository
    {
        Task<int> CreateRegistrationType(RegistrationType input);
        Task<bool> UpdateRegistrationType(RegistrationType input);
        Task<bool> DeleteRegistrationType(RegistrationType input);
        Task<RegistrationType?> Get(int id);
        Task<PagedResponse<RegistrationType>> Search(Expression<Func<RegistrationType, bool>> predicate, int pageNumber, int pageSize, bool enablePagination);
    }
}
