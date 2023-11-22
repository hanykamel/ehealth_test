using EHealth.ManageItemLists.Domain.GlobelPackageTypes;
using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IGlobelPackageTypeRepository
    {
        Task<int> Create(GlobelPackageType input);
        Task<bool> Update(GlobelPackageType input);
        Task<bool> Delete(GlobelPackageType input);
        Task<GlobelPackageType?> Get(int id);
        Task<PagedResponse<GlobelPackageType>> Search(Expression<Func<GlobelPackageType, bool>> predicate, int pageNumber, int pageSize, bool enablePagination);

    }
}
