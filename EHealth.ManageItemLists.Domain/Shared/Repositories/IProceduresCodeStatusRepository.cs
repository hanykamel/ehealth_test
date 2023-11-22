using EHealth.ManageItemLists.Domain.ProceduresCodesStatus;
using EHealth.ManageItemLists.Domain.Shared.Pagination;


namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IProceduresCodeStatusRepository
    {
        Task<int> CreateProceduresCodeStatus(ProceduresCodeStatus input);
        Task<bool> UpdateProceduresCodeStatus(ProceduresCodeStatus input);
        Task<bool> DeleteProceduresCodeStatus(ProceduresCodeStatus input);
        Task<ProceduresCodeStatus?> Get(int id);
        Task<PagedResponse<ProceduresCodeStatus>> Search(int? Id, string? Code, string? CodeStatusDescAr, string? CodeStatusDescEng, bool Active, int pageNumber, int pageSize);
    }
}
