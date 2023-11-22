using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.PackageTypes;
using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
namespace EHealth.ManageItemLists.Infrastructure.Repositories.Lookups
{
    public class GetPackageTypesRepository
    {
        private readonly EHealthDbContext eHealthDbContext;

        //add constructor here and use DI to inject your dependencies, and inject your context
        public GetPackageTypesRepository(EHealthDbContext eHealthDbContext)
        {
            this.eHealthDbContext = eHealthDbContext;
        }




        //add method to search package types
        public IQueryable<PackageType> GetAll()
        {
            //implement your logic to get all package types
            return eHealthDbContext.PackageTypes;
            
                    }
    }
}
