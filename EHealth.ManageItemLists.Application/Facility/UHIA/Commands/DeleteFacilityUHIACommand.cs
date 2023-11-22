using MediatR;

namespace EHealth.ManageItemLists.Application.Facility.UHIA.Commands
{
    public record DeleteFacilityUHIACommand(Guid Id) : IRequest<bool>;
}
