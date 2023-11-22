using MediatR;

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA
{
    public class DeleteConsAndDevsUHIACommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
