using MediatR;

namespace EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Commands
{
    public class DeleteDevicesAndAssetsUHIACommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
