using EHealth.ManageItemLists.Application.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Procedure.ICHI.DTOs
{
    public class CreateProcedureICHIPriceDto
    {
        public Guid ProcedureICHIId { get; set; }
        public List<CreateItemListPriceDto> ItemListPrices { get; set; } = new List<CreateItemListPriceDto>();
    }
}
