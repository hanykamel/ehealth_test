using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHealth.ManageItemLists.Domain.PackageTypes;
using EHealth.ManageItemLists.Domain.Shared.Pagination;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IGetPackageTypesRepository
    {
        //add method to search package types
        Task<PagedResponse<PackageType>> GetAll();


    }
}
