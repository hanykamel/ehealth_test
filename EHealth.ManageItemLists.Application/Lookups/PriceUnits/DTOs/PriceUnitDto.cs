using EHealth.ManageItemLists.Application.ItemListPrices.DTOs;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.PriceUnits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.PriceUnits.DTOs
{
    public class PriceUnitDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string NameAr { get; private set; }
        public string NameEN { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionEN { get; private set; }
        public int ResourceUnitOfCostValue { get; set; }

        public static PriceUnitDto FromPriceUnit(PriceUnit input) =>
        input is not null ? new PriceUnitDto
        {
            Id = input.Id,
            Code = input.Code,
            NameAr = input.NameAr,
            NameEN = input.NameEN,
            DefinitionAr = input.DefinitionAr,
            DefinitionEN = input.DefinitionEN,
            ResourceUnitOfCostValue = input.ResourceUnitOfCostValue
        } : null;
    }
}
