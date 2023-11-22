using EHealth.ManageItemLists.Application.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.DTOs
{
    public class UpdateDevicesAndAssetsUHIAPriceDto
    {
        public Guid DevicesAndAssetsUHIAId { get; set; }
        public List<UpdateItemListPriceDto> ItemListPrices { get; set; } = new List<UpdateItemListPriceDto>();
    }
}
