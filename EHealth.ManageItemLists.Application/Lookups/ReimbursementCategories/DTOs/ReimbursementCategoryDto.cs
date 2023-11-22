using EHealth.ManageItemLists.Application.Lookups.LocalUnitOfMeasures.DTOs;
using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
using EHealth.ManageItemLists.Domain.ReimbursementCategories;
using EHealth.ManageItemLists.Domain.UnitsTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.ReimbursementCategories.DTOs
{
    public class ReimbursementCategoryDto
    {
        public int? Id { get; set; }
        public string? Code { get; set; }
        public string NameAr { get; private set; }
        public string NameEn { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionEn { get; private set; }
        public bool IsDeleted { get; set; }

        public static ReimbursementCategoryDto FromReimbursementCategory(ReimbursementCategory input) =>
        input is not null ? new ReimbursementCategoryDto
      {
          Id = input.Id,
          Code = input.Code,
          NameAr = input.NameAr,
          NameEn = input.NameENG,
          DefinitionAr = input.DefinitionAr,
          DefinitionEn = input.DefinitionENG,
          IsDeleted = input.IsDeleted
      } : null;
    }
}
