using EHealth.ManageItemLists.Application.Facility.UHIA.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Facility.UHIA.Queries
{
    public class FacilityUHIAGetByIdQuery : IRequest<FacilityUHIADto>
    {
        public Guid Id { get; set; }
    }
}
