using EHealth.ManageItemLists.Domain.DispensingLocations;
using EHealth.ManageItemLists.Domain.Shared.Pagination;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IDispensingLocationRepository
    {
        Task<int> CreateDispensingLocation(DispensingLocation input);
        Task<bool> UpdateDispensingLocation(DispensingLocation input);
        Task<bool> DeleteDispensingLocation(DispensingLocation input);
        Task<DispensingLocation?> Get(int id);
        Task<PagedResponse<DispensingLocation>> Search(int id,string code, string? nameAr, string? nameENG, string? DefinitionAr, string? DefinitionENG, bool Active, int pageNumber, int pageSize);
    }
}
