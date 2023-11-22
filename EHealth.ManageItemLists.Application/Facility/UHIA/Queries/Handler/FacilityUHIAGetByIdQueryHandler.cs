using EHealth.ManageItemLists.Application.Facility.UHIA.DTOs;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Facility.UHIA.Queries.Handler
{
    public class FacilityUHIAGetByIdQueryHandler : IRequestHandler<FacilityUHIAGetByIdQuery, FacilityUHIADto>
    {
        private readonly IFacilityUHIARepository _facilityUHIARepository;
        public FacilityUHIAGetByIdQueryHandler(IFacilityUHIARepository facilityUHIARepository)
        {
            _facilityUHIARepository = facilityUHIARepository;
        }
        public async Task<FacilityUHIADto> Handle(FacilityUHIAGetByIdQuery request, CancellationToken cancellationToken)
        {
            var res = await FacilityUHIA.Get(request.Id, _facilityUHIARepository);
            return FacilityUHIADto.FromFacilityUHIA(res);
        }
    }
}
