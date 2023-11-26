using DocumentFormat.OpenXml.Vml.Spreadsheet;
using EHealth.ManageItemLists.Application.Drugs.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Lookups.UnitsTypes.DTOs;
using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.PackageItems.DTOs
{
    public class GetAllDrugsDTO
    {

        public string? ProprietaryName { get; set; }
        public string? EHealthDrugCode { get; set; }
        public string? LocalDrugCode { get; set; }
        public double? SubUnitPrice { get; set; }
        public UnitsTypeDto? SubUnitOfMeasure { get; set; }

        public static GetAllDrugsDTO FromGetAllDrugsDTO(DrugUHIA input) =>
            new GetAllDrugsDTO
            {
                ProprietaryName = input.ProprietaryName,
                EHealthDrugCode = input.EHealthDrugCode,
                LocalDrugCode = input.LocalDrugCode,
                SubUnitPrice = DrugPriceDto.FromDrugPrice(input.DrugPrices.OrderByDescending(o=>o.EffectiveDateFrom).FirstOrDefault())?.SubUnitPrice,
                SubUnitOfMeasure = UnitsTypeDto.FromUnitsType(input.SubUnit)
            };

    }
}
