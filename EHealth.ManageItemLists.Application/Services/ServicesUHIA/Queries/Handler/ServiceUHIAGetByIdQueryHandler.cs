using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.Queries.Handler
{
    public class ServiceUHIAGetByIdQueryHandler : IRequestHandler<ServiceUHIAGetByIdQuery, ServiceGetByIdDto>
    {
        private readonly IServiceUHIARepository _serviceUHIARepository;
      
        public ServiceUHIAGetByIdQueryHandler(IServiceUHIARepository serviceUHIARepository)
        {
            _serviceUHIARepository = serviceUHIARepository;
        }
        public async Task<ServiceGetByIdDto> Handle(ServiceUHIAGetByIdQuery request, CancellationToken cancellationToken)
        {
            var res = await ServiceUHIA.Get(request.Id, _serviceUHIARepository);
             return ServiceGetByIdDto.FromServiceUHIA(res);
          
        }
    }
}
