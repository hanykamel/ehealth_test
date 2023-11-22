using EHealth.ManageItemLists.Domain.Pre_authorizationProtocol;
using EHealth.ManageItemLists.Domain.Shared.Pagination;


namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IPreAuthorizationProtocolRepository
    {
        Task<int> CreatePreAuthorizationProtocol(PreAuthorizationProtocol input);
        Task<bool> UpdatePreAuthorizationProtocol(PreAuthorizationProtocol input);
        Task<bool> DeletePreAuthorizationProtocol(PreAuthorizationProtocol input);
        Task<PreAuthorizationProtocol?> Get(int id);
        Task<PagedResponse<PreAuthorizationProtocol>> Search(int id,string? code, string? nameAr, string? nameENG, string? DefinitionAr, string? DefinitionENG, bool Active, int pageNumber, int pageSize);
    }
}
