using EHealth.ManageItemLists.Application.Lookups.Type.DTOs;
using EHealth.ManageItemLists.Domain.ItemListTypes;
using EHealth.ManageItemLists.Domain.UnitsTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.UnitsTypes.DTOs
{
    public class UnitsTypeDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string UnitAr { get; private set; }
        public string UnitEn { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionEn { get; private set; }
        public bool IsDeleted { get; set; }

        public static UnitsTypeDto FromUnitsType(UnitsType unitsType) =>
        unitsType is not null ? new UnitsTypeDto
        {
            Id = unitsType.Id,
            Code = unitsType.Code,
            UnitAr = unitsType.UnitAr,
            UnitEn = unitsType.UnitEn,
            DefinitionAr = unitsType.DefinitionAr,
            DefinitionEn = unitsType.DefinitionEn,
            IsDeleted = unitsType.IsDeleted
        } : null;
    }
}
