using EHealth.ManageItemLists.Application.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs
{
    public class UpdateConsAndDevUHIAPriceDto
    {
        public Guid ConsAndDevUHIAId { get; set; }
        public List<UpdateItemListPriceDto> ItemListPrices { get; set; } = new List<UpdateItemListPriceDto>();
    }
}
