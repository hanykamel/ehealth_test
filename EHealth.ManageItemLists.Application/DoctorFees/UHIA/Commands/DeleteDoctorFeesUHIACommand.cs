using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands
{
    public class DeleteDoctorFeesUHIACommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
