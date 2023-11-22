using EHealth.ManageItemLists.Application.Shared.DTOs;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs
{
    public class CreateServicesUHIAPriceDto
    {

        
        public Guid ServiceUHIAId { get; set; }
        public List<CreateItemListPriceDto> ItemListPrices { get; set; } = new List<CreateItemListPriceDto>();
    }
}
