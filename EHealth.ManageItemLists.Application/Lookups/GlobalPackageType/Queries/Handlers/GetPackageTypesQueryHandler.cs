using EHealth.ManageItemLists.Application.Lookups.PackageType.DTOs;
using EHealth.ManageItemLists.Application.Lookups.PackageType.Queries;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
namespace EHealth.ManageItemLists.Application.Lookups.GlobalPackageType.Queries.Handlers
{
    public class GetPackageTypesQueryHandler : IRequestHandler<GetPackageTypesQuery, PagedResponse<PackageTypeDto>>
    {
        private readonly IGetPackageTypesRepository getPackageTypesRepository;

        // add constructor here and use DI to inject your dependencies, and inject your repository
        public GetPackageTypesQueryHandler(IGetPackageTypesRepository getPackageTypesRepository)
        {
            this.getPackageTypesRepository = getPackageTypesRepository;
        }
        public Task<PagedResponse<PackageTypeDto>> Handle(GetPackageTypesQuery request, CancellationToken cancellationToken)
        {
            throw                 new NotImplementedException();
          
        }
    }
}
