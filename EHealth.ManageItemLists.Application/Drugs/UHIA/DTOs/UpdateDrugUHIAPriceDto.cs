using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.DTOs
{
    public class UpdateDrugUHIAPriceDto
    {
        public Guid Id { get; set; }
        public List<UpdateDrugPriceDto> drugPrices { get; set; } = new List<UpdateDrugPriceDto>();
    }
}
