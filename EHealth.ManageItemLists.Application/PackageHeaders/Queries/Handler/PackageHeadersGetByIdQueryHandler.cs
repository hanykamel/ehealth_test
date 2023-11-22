using EHealth.ManageItemLists.Application.PackageHeaders.DTOs;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Queries;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.PackageHeaders.Queries.Handler
{
    using PackageHeader = EHealth.ManageItemLists.Domain.Packages.PackageHeaders.PackageHeader;
    public class PackageHeadersGetByIdQueryHandler : IRequestHandler<PackageHeadersGetByIdQuery, PackageHeadersGetByIdDTO>
    {
        
        private readonly IPackageHeaderRepository _packageHeaderRepository;

        public PackageHeadersGetByIdQueryHandler(IPackageHeaderRepository packageHeaderRepository)
        {
            _packageHeaderRepository = packageHeaderRepository;
        }

        public async Task<PackageHeadersGetByIdDTO> Handle(PackageHeadersGetByIdQuery request, CancellationToken cancellationToken)
        {
            var res = await PackageHeader.Get(request.Id,_packageHeaderRepository );
            return PackageHeadersGetByIdDTO.FromPackageHeader(res);

        }
    }
}
