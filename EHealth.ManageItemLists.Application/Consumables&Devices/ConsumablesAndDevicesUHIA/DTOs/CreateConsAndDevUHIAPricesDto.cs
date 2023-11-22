using EHealth.ManageItemLists.Application.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs
{
    public class CreateConsAndDevUHIAPricesDto
    {
        public Guid ConsumablesAndDevicesUHIAId { get; set; }
        public List<CreateItemListPriceDto> ItemListPrices { get; set; } = new List<CreateItemListPriceDto>();
    }
}
