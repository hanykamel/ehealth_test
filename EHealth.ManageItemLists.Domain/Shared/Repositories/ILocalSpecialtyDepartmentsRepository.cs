using EHealth.ManageItemLists.Domain.LocalSpecialtyDepartments;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface ILocalSpecialtyDepartmentsRepository
    {
        Task<int> CreateLocalSpecialtyDepartments(LocalSpecialtyDepartment input);
        Task<bool> UpdateLocalSpecialtyDepartments(LocalSpecialtyDepartment input);
        Task<bool> DeleteLocalSpecialtyDepartments(LocalSpecialtyDepartment input);
        Task<LocalSpecialtyDepartment?> Get(int id);
        Task<PagedResponse<LocalSpecialtyDepartment>> Search(Expression<Func<LocalSpecialtyDepartment, bool>> predicate, int pageNumber, int pageSize, bool enablePagination);
    }
}
