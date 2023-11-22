using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Procedure.ICHI.Commands
{
    public class DeleteProcedureICHICommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
