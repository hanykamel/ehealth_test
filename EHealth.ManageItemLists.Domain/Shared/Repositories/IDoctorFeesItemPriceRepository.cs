using EHealth.ManageItemLists.Domain.DoctorFees.ItemPrice;
using EHealth.ManageItemLists.Domain.Resource.ItemPrice;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IDoctorFeesItemPriceRepository
    {
        Task<int> Create(DoctorFeesItemPrice input);
        Task<bool> Update(DoctorFeesItemPrice input);
        Task<bool> Delete(DoctorFeesItemPrice input);
        Task<DoctorFeesItemPrice?> Get(int id);
        Task<PagedResponse<DoctorFeesItemPrice>> Search(Expression<Func<DoctorFeesItemPrice, bool>> predicate, int pageNumber, int pageSize);
    }
}
