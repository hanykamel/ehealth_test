using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageConsumablesAndDevices.Commnads
{
    public class DeleteSharedItemsPackageConsumablesAndDevicesCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
