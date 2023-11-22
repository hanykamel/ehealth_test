using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.DTOs;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;

namespace EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Queries.Handler
{
    public class DevicesAndAssetsUHIAGetByIdQueryHandler : IRequestHandler<DevicesAndAssetsUHIAGetByIdQuery, DevicesAndAssetsUHIAGetByIdDto>
    {
        private readonly IDevicesAndAssetsUHIARepository _devicesAndAssetsUHIARepository;
        public DevicesAndAssetsUHIAGetByIdQueryHandler(IDevicesAndAssetsUHIARepository devicesAndAssetsUHIARepository)
        {
            _devicesAndAssetsUHIARepository = devicesAndAssetsUHIARepository;
        }
        public async Task<DevicesAndAssetsUHIAGetByIdDto> Handle(DevicesAndAssetsUHIAGetByIdQuery request, CancellationToken cancellationToken)
        {
            var res = await DevicesAndAssetsUHIA.Get(request.Id, _devicesAndAssetsUHIARepository);
            return DevicesAndAssetsUHIAGetByIdDto.FromDevicesAndAssetsUHIAGetById(res);
        }
    }
}
