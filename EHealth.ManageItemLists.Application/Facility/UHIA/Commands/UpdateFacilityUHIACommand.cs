using EHealth.ManageItemLists.Application.Facility.UHIA.DTOs;
using EHealth.ManageItemLists.Application.ItemLists.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Facility.UHIA.Commands
{
    public record UpdateFacilityUHIACommand(UpdateFacilityUHIADto UpdateFacilityUHIADto) : IRequest<FacilityUHIADto>;
}
