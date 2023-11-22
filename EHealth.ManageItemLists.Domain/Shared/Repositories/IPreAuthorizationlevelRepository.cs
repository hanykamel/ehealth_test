using EHealth.ManageItemLists.Domain.Pre_authorizationlevel;
using EHealth.ManageItemLists.Domain.Shared.Pagination;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IPreAuthorizationlevelRepository
    {
        Task<int> CreatePreAuthorizationlevel(PreAuthorizationlevel input);
        Task<bool> UpdatePreAuthorizationlevel(PreAuthorizationlevel input);
        Task<bool> DeletePreAuthorizationlevel(PreAuthorizationlevel input);
        Task<PreAuthorizationlevel?> Get(int id);
        Task<PagedResponse<PreAuthorizationlevel>> Search(int id, string? code, string? levelAr, string? levelENG, string? DefinitionAr, string? DefinitionENG, bool Active, int pageNumber, int pageSize);
    }
}
