using EHealth.ManageItemLists.Application.Lookups.PriceUnits.DTOs;
using EHealth.ManageItemLists.Domain.PriceUnits;
using EHealth.ManageItemLists.Domain.UnitOfTheDoctor_sfees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.UnitOfTheDoctorFees.DTOs
{
    public class UnitOfTheDoctorFeesDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string NameAr { get; private set; }
        public string NameEN { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionEN { get; private set; }

        public static UnitOfTheDoctorFeesDto FromUnitOfTheDoctorFees(UnitDOF input) =>
        input is not null ? new UnitOfTheDoctorFeesDto
        {
            Id = input.Id,
            Code = input.Code,
            NameAr = input.NameAr,
            NameEN = input.NameEN,
            DefinitionAr = input.DefinitionAr,
            DefinitionEN = input.DefinitionEN,
        } : null;
    }
}
