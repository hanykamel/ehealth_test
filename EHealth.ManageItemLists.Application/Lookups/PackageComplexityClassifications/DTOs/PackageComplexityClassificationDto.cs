using EHealth.ManageItemLists.Application.Lookups.PriceUnits.DTOs;
using EHealth.ManageItemLists.Domain.PackageComplexityClassifications;
using EHealth.ManageItemLists.Domain.PriceUnits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.PackageComplexityClassifications.DTOs
{
    public class PackageComplexityClassificationDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string ComplexityAr { get; private set; }
        public string ComplexityEn { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionEn { get; private set; }

        public static PackageComplexityClassificationDto FromPackageComplexityClassification(PackageComplexityClassification input) =>
        input is not null ? new PackageComplexityClassificationDto
        {
            Id = input.Id,
            Code = input.Code,
            ComplexityAr = input.ComplexityAr,
            ComplexityEn = input.ComplexityEn,
            DefinitionAr = input.DefinitionAr,
            DefinitionEn = input.DefinitionEn,
        } : null;
    }
}
