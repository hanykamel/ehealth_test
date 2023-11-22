using EHealth.ManageItemLists.Application.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs
{
    public class UpdateServicesUHIAPriceDto
    {
        public Guid ServiceUHIAId { get; set; }
        public List<UpdateItemListPriceDto> ItemListPrices { get; set; } = new List<UpdateItemListPriceDto>();
    }
}
