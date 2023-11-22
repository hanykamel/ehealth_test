using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Queries
{
    public class ConsAndDevUHIAGetByIdQuery : IRequest<ConsAndDtoUHIAGetByIdDto>
    {
        public Guid Id { get; set; }
    }
}
