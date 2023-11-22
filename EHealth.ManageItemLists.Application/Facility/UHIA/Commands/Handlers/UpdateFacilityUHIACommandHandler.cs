using EHealth.ManageItemLists.Application.Facility.UHIA.DTOs;
using EHealth.ManageItemLists.Application.ItemLists.Commands;
using EHealth.ManageItemLists.Application.ItemLists.DTOs;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Facility.UHIA.Commands.Handlers
{
    public class UpdateFacilityUHIACommandHandler : IRequestHandler<UpdateFacilityUHIACommand, FacilityUHIADto>
    {
        private readonly IFacilityUHIARepository _facilityUHIAsRepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public UpdateFacilityUHIACommandHandler(IFacilityUHIARepository facilityUHIAsRepository, IValidationEngine validationEngine, IIdentityProvider identityProvider)
        {
            _facilityUHIAsRepository = facilityUHIAsRepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }

        public async Task<FacilityUHIADto> Handle(UpdateFacilityUHIACommand request, CancellationToken cancellationToken)
        {
            var facilityUHIA = await FacilityUHIA.Get(request.UpdateFacilityUHIADto.Id, _facilityUHIAsRepository);

            if(facilityUHIA == null)
                throw new DataNotFoundException();

            facilityUHIA.SetCode(request.UpdateFacilityUHIADto.EHealthCode);
            facilityUHIA.SetCategoryId(request.UpdateFacilityUHIADto.CategoryId);
            facilityUHIA.SetSubCategoryId(request.UpdateFacilityUHIADto.SubCategoryId);
            facilityUHIA.SetDataEffectiveDateTo(request.UpdateFacilityUHIADto.DataEffectiveDateTo);
            facilityUHIA.SetDataEffectiveDateFrom(request.UpdateFacilityUHIADto.DataEffectiveDateFrom);
            facilityUHIA.SetDescriptorEn(request.UpdateFacilityUHIADto.DescriptorEn);
            facilityUHIA.SetDescriptorAr(request.UpdateFacilityUHIADto.DescriptorAr);
            facilityUHIA.SetOccupancyRate(request.UpdateFacilityUHIADto.OccupancyRate);
            facilityUHIA.SetOperatingDaysPerMonth(request.UpdateFacilityUHIADto.OperatingDaysPerMonth);
            facilityUHIA.SetOperatingRateInHoursPerDay(request.UpdateFacilityUHIADto.OperatingRateInHoursPerDay);


            await facilityUHIA.Update(_facilityUHIAsRepository, _validationEngine, _identityProvider.GetUserName());
            return FacilityUHIADto.FromFacilityUHIA(facilityUHIA);
        }
    }
}
