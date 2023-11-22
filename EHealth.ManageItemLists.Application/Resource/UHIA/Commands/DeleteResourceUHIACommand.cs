using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Resource.UHIA.Commands
{
    public class DeleteResourceUHIACommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
