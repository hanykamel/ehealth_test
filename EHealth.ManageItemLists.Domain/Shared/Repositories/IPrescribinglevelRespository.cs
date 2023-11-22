using EHealth.ManageItemLists.Domain.Prescribinglevels;
using EHealth.ManageItemLists.Domain.Shared.Pagination;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IPrescribinglevelRespository
    {
        Task<int> CreatePrescribinglevel(Prescribinglevel input);
        Task<bool> UpdatePrescribinglevel(Prescribinglevel input);
        Task<bool> DeletePrescribinglevel(Prescribinglevel input);
        Task<Prescribinglevel?> Get(int id);
        Task<PagedResponse<Prescribinglevel>> Search(int? id, string? code, string? levelAr, string? levelENG, string? DefinitionAr, string? DefinitionENG, bool Active, int pageNumber, int pageSize);
    }
}
