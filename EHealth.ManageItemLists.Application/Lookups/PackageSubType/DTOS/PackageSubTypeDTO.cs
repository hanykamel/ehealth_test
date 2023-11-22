using EHealth.ManageItemLists.Domain.PackageSubTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.PackageSubType.DTOS
{
    using PackageSubType = EHealth.ManageItemLists.Domain.PackageSubTypes.PackageSubType;
    public class PackageSubTypeDTO
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEN { get; set; }
        public int PackageTypeId { get; set; }

        public static PackageSubTypeDTO FromPackageSubType(PackageSubType input) =>
        input is not null ? new PackageSubTypeDTO
        {
         Id=input.Id,
         NameAr=input.NameAr,
         NameEN=input.NameEN,
         PackageTypeId=input.PackageTypeId
        } : null;
    }
}
