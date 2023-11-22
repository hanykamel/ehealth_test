using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.DTOs;
using EHealth.ManageItemLists.Application.DoctorFees.UHIA.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.Queries
{
    public class DoctorFeesUHIAGetByIdQuery : IRequest<DoctorFeesUHIAGetByIdDto>
    {
        public Guid Id { get; set; }
    }
}
