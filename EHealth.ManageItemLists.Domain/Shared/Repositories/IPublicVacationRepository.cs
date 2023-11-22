using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.PublicVacations;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IPublicVacationRepository
    {
        Task<int> Create(PublicVacation input);
        Task<bool> Update(PublicVacation input);
        Task<bool> Delete(PublicVacation input);
        Task<PublicVacation?> Get(int id);
        Task<PagedResponse<PublicVacation>> Search(int Id, string? NameAr, string? NameEn, DateTime FromDate, DateTime ToDate, int pageNumber, int pageSize);

    }
}
