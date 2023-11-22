using EHealth.ManageItemLists.Application.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.DTOs
{
    public class CreateDrugUHIAPricesDto
    {
        public Guid Id { get; set; }
        public List<CreateDrugPriceDto> ItemListPrices { get; set; } = new List<CreateDrugPriceDto>();
    }
}
