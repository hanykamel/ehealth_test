using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.Queries
{
    public class ServiceUHIAGetByIdQuery :  IRequest<ServiceGetByIdDto>
    {
        public Guid Id { get; set; }
    }
}
