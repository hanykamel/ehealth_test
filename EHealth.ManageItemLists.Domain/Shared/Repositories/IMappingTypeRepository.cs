using EHealth.ManageItemLists.Domain.MappingTypes;
using EHealth.ManageItemLists.Domain.Shared.Pagination;


namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IMappingTypeRepository
    {
        Task<int> CreateMappingType(MappingType input);
        Task<bool> UpdateMappingType(MappingType input);
        Task<bool> DeleteMappingType(MappingType input);
        Task<MappingType?> Get(int id);
        Task<PagedResponse<MappingType>> Search(int? id,string? code, string? mappingTypeAr, string? mappingTypeENG, string? DefinitionAr, string? DefinitionENG, bool Active, int pageNumber, int pageSize);
    }
}
