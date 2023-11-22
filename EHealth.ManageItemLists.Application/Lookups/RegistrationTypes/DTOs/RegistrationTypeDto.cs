using EHealth.ManageItemLists.Application.Lookups.LocalUnitOfMeasures.DTOs;
using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
using EHealth.ManageItemLists.Domain.RegistrationTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.RegistrationTypes.DTOs
{
    public class RegistrationTypeDto
    {
        public int? Id { get; set; }
        public string? Code { get; set; }
        public string RegistrationTypeAr { get; private set; }
        public string RegistrationTypeEn { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionEn { get; private set; }
        public bool IsDeleted { get; set; }


        public static RegistrationTypeDto FromRegistrationType(RegistrationType input) =>
      input is not null ? new RegistrationTypeDto
      {
          Id = input.Id,
          Code = input.Code,
          RegistrationTypeAr = input.RegistrationTypeAr,
          RegistrationTypeEn = input.RegistrationTypeENG,
          DefinitionAr = input.DefinitionAr,
          DefinitionEn = input.DefinitionENG,
          IsDeleted = input.IsDeleted
      }:null;
    }
}
