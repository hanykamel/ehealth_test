using MediatR;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands
{
    public class DeleteServicesUHIACommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
