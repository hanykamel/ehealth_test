using EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI;
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
    public interface IProcedureICHIRepository
    {
        Task<Guid> Create(ProcedureICHI input);
        Task<bool> Update(ProcedureICHI input);
        Task<bool> Delete(ProcedureICHI input);
        Task<ProcedureICHI?> Get(Guid id);
        Task<PagedResponse<ProcedureICHI>> Search(Expression<Func<ProcedureICHI, bool>> predicate, int pageNumber, int pageSize,bool enablePagination, string? orderBy, bool? ascending);
        Task<bool> IsItemLIstBusy(int itemListId);
    }
}
