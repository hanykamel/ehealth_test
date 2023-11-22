using EHealth.ManageItemLists.Domain.GlobelPackageTypes;
using EHealth.ManageItemLists.Domain.PackageSpecialties;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IPackageSpecialtiesRepository
    {
        Task<int> Create(PackageSpecialty input);
        Task<bool> Update(PackageSpecialty input);
        Task<bool> Delete(PackageSpecialty input);
        Task<PackageSpecialty?> Get(int id);
        Task<PagedResponse<PackageSpecialty>> Search(Expression<Func<PackageSpecialty, bool>> predicate, int pageNumber, int pageSize, bool enablePagination);

    }
}
