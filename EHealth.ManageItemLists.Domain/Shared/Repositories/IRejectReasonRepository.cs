using EHealth.ManageItemLists.Domain.PackageComplexityClassifications;
using EHealth.ManageItemLists.Domain.Rejectreasons;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IRejectReasonRepository
    {
        Task<int> Create(RejectReason input);
        Task<bool> Update(RejectReason input);
        Task<bool> Delete(RejectReason input);
        Task<RejectReason?> Get(int id);
        Task<PagedResponse<RejectReason>> Search(int? id, string? Code, string? rejectReasonAr, string? rejectReasonEn, bool Active, int pageNumber, int pageSize);

    }
}
