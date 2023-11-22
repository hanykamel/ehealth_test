using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Queries.Handler
{
    public class ConsAndDevsUHIAGetByIdQueryHandler : IRequestHandler<ConsAndDevUHIAGetByIdQuery, ConsAndDtoUHIAGetByIdDto>
    {
        private readonly IConsumablesAndDevicesUHIARepository _consumablesAndDevicesUHIARepository;

        public ConsAndDevsUHIAGetByIdQueryHandler(IConsumablesAndDevicesUHIARepository consumablesAndDevicesUHIARepository)
        {
            _consumablesAndDevicesUHIARepository = consumablesAndDevicesUHIARepository;
        }
        public async Task<ConsAndDtoUHIAGetByIdDto> Handle(ConsAndDevUHIAGetByIdQuery request, CancellationToken cancellationToken)
        {
            var res = await Domain.ConsumablesAndDevices.ConsumablesAndDevicesUHIA.Get(request.Id, _consumablesAndDevicesUHIARepository);
            return ConsAndDtoUHIAGetByIdDto.FromConsAndDevsUHIAGetById(res);

        }
      
    }
}
