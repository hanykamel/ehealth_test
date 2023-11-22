using EHealth.ManageItemLists.Domain.Packages.PackageHeaders;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IPackageHeaderRepository
    {
        Task<Guid> Create(PackageHeader input);
        Task<bool> Update(PackageHeader input);
        Task<bool> Delete(PackageHeader input);
        Task<PackageHeader?> Get(Guid id);
        Task<PagedResponse<PackageHeader>> Search(Expression<Func<PackageHeader, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending);
    }
}
