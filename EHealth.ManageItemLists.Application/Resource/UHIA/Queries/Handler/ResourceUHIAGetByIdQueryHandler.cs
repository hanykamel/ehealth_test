using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Resource.UHIA.DTOs;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Resource.UHIA.Queries.Handler
{
    public class ResourceUHIAGetByIdQueryHandler : IRequestHandler<ResourceUHIAGetByIdQuery, ResourceUHIAByIdDto>
    {
        private readonly IResourceUHIARepository _resourceUHIARepository;
        public ResourceUHIAGetByIdQueryHandler(IResourceUHIARepository resourceUHIARepository)
        {
            _resourceUHIARepository = resourceUHIARepository;
        }
        public async Task<ResourceUHIAByIdDto> Handle(ResourceUHIAGetByIdQuery request, CancellationToken cancellationToken)
        {
            var res = await ResourceUHIA.Get(request.Id, _resourceUHIARepository);
            return ResourceUHIAByIdDto.FromResourceUHIA(res);
        }
    }
}
