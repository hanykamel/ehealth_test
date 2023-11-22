
using EHealth.ManageItemLists.Domain.GlobelPackageTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.GlobalPackageType.DTOs
{
    public class GlobalPackageTypeDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string GlobalTypeAr { get; private set; }
        public string GlobalTypeEn { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionEn { get; private set; }

        public static GlobalPackageTypeDTO FromGlobalPackageType(GlobelPackageType input) =>
    input is not null ? new GlobalPackageTypeDTO
    {
        Id = input.Id,
        Code = input.Code,
        GlobalTypeAr = input.GlobalTypeAr,
        GlobalTypeEn = input.GlobalTypeEn,
        DefinitionAr = input.DefinitionAr,
        DefinitionEn = input.DefinitionEn
    } : null;
    }
}
