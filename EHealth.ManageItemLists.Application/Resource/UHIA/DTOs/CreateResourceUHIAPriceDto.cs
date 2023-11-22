using EHealth.ManageItemLists.Application.Resource.ItemPrice.DTOs;
using EHealth.ManageItemLists.Application.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Resource.UHIA.DTOs
{
    public class CreateResourceUHIAPriceDto
    {
        public Guid ResourceUHIAId { get; set; }
        public List<CreateResourceItemPriceDto> ResourceItemPrices { get; set; } = new List<CreateResourceItemPriceDto>();
    }
}
