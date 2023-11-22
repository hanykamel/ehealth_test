using EHealth.ManageItemLists.Application.Lookups.PackageComplexityClassifications.DTOs;
using EHealth.ManageItemLists.Domain.PackageComplexityClassifications;
using EHealth.ManageItemLists.Domain.PackageSpecialties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EHealth.ManageItemLists.Application.Lookups.PackageSpecialty.DTOs
{
    using PackageSpecialty = EHealth.ManageItemLists.Domain.PackageSpecialties.PackageSpecialty;
    public class PackageSpecialtyDto
    {
        public int Id { get; set; }
        public string SpecialtyAr { get; private set; }
        public string SpecialtyEn { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionEn { get; private set; }

        public static PackageSpecialtyDto FromPackageSpecialty(PackageSpecialty input) =>
            input is not null ? new PackageSpecialtyDto
            {
                Id = input.Id,
                SpecialtyAr = input.SpecialtyAr,
                SpecialtyEn = input.SpecialtyEn,
                DefinitionAr = input.DefinitionAr,
                DefinitionEn = input.DefinitionEn
            } : null;
    }

}
