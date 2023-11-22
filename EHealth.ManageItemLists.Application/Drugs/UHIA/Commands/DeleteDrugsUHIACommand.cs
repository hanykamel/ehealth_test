using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.Commands
{
    public class DeleteDrugsUHIACommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
