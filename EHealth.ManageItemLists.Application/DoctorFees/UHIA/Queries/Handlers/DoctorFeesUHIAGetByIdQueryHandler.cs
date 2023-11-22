using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.DTOs;
using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Queries;
using EHealth.ManageItemLists.Application.DoctorFees.UHIA.DTOs;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.DoctorFees.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.Queries.Handlers
{
    public class DoctorFeesUHIAGetByIdQueryHandler : IRequestHandler<DoctorFeesUHIAGetByIdQuery, DoctorFeesUHIAGetByIdDto>
    {
        private readonly IDoctorFeesUHIARepository _doctorFeesUHIARepository;
        public DoctorFeesUHIAGetByIdQueryHandler(IDoctorFeesUHIARepository doctorFeesUHIARepository)
        {
            _doctorFeesUHIARepository = doctorFeesUHIARepository;
        }
        public async Task<DoctorFeesUHIAGetByIdDto> Handle(DoctorFeesUHIAGetByIdQuery request, CancellationToken cancellationToken)
        {
            var res = await DoctorFeesUHIA.Get(request.Id, _doctorFeesUHIARepository);
            return DoctorFeesUHIAGetByIdDto.FromDoctorFeesUHIAGetById(res);
        }
    }
}
