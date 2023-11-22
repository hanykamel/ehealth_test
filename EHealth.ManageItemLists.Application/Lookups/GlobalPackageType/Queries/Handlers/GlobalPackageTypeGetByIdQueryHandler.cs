using EHealth.ManageItemLists.Application.Lookups.GlobalPackageType.DTOs;
using EHealth.ManageItemLists.Domain.GlobelPackageTypes;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.GlobalPackageType.Queries.Handlers
{
    public class GlobalPackageTypeGetByIdQueryHandler : IRequestHandler<GlobalPackageTypeGetByIdQuery, GlobalPackageTypeDTO>
    {
        private readonly IGlobelPackageTypeRepository _globelPackageTypeRepository;

        public GlobalPackageTypeGetByIdQueryHandler(IGlobelPackageTypeRepository globelPackageTypeRepository) 
        {
            _globelPackageTypeRepository = globelPackageTypeRepository;
        }
        public async Task<GlobalPackageTypeDTO> Handle(GlobalPackageTypeGetByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await GlobelPackageType.Get(request.id, _globelPackageTypeRepository);
            return GlobalPackageTypeDTO.FromGlobalPackageType(result);
        }
    }
}
