using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.DTOs;
using MediatR;

namespace EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Queries
{
    public class DevicesAndAssetsUHIAGetByIdQuery : IRequest<DevicesAndAssetsUHIAGetByIdDto>
    {
        public Guid Id { get; set; }
    }
}
