using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs;
using EHealth.ManageItemLists.Application.Procedure.ICHI.DTOs;
using MediatR;

namespace EHealth.ManageItemLists.Application.Procedure.ICHI.Queries
{
    public class ProcedureICHIGetByIdQuery : IRequest<ProcedureICHIGetByIdDto>
    {
        public Guid Id { get; set; }
    }
}
