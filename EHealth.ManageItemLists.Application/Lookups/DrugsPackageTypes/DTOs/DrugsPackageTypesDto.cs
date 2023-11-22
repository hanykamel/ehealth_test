using EHealth.ManageItemLists.Application.Lookups.LocalUnitOfMeasures.DTOs;
using EHealth.ManageItemLists.Domain.DrugsPackageTypes;
using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.DrugsPackageTypes.DTOs
{
    public class DrugsPackageTypesDto
    {
        public int? Id { get; set; }
        public string? Code { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? DefinitionAr { get; set; }
        public string? DefinitionEn { get; set; }
        public bool IsDeleted { get; set; }
        public static DrugsPackageTypesDto FromDrugsPackageType(DrugsPackageType input) =>
      new DrugsPackageTypesDto
      {
          Id = input.Id,
          Code = input.Code,
          NameAr = input.NameAr,
          NameEn = input.NameEN,
          DefinitionAr = input.DefinitionAr,
          DefinitionEn = input.DefinitionEN,
          IsDeleted = input.IsDeleted
      };
    }
}
