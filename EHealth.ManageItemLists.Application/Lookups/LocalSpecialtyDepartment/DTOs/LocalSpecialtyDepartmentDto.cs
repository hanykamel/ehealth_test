using EHealth.ManageItemLists.Application.Lookups.LocalUnitOfMeasures.DTOs;
using EHealth.ManageItemLists.Domain.LocalSpecialtyDepartments;
using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.LocalSpecialtyDepartment.DTOs
{
    public class LocalSpecialtyDepartmentDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string LocalSpecialityAr { get;  set; }
        public string LocalSpecialityEn { get;  set; }
        public string? DefinitionAr { get;  set; }
        public string? DefinitionEn { get;  set; }
        public bool IsDeleted { get; set; }

        public static LocalSpecialtyDepartmentDto FromLLocalSpecialityDepartment(Domain.LocalSpecialtyDepartments.LocalSpecialtyDepartment input) =>
     input is not null ? new LocalSpecialtyDepartmentDto
  {
      Id = input.Id,
      Code = input.Code,
      LocalSpecialityAr = input.LocalSpecialityAr,
      LocalSpecialityEn = input.LocalSpecialityENG,
      DefinitionAr = input.LocalSpecialityENG,
      DefinitionEn = input.DefinitionENG,
      IsDeleted =input.IsDeleted

  }:null;
    }
}
