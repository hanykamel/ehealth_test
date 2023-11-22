using EHealth.ManageItemLists.Application.Lookups.UnitOfTheDoctorFees.DTOs;
using EHealth.ManageItemLists.Domain.DoctorFees.ItemPrice;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.UnitOfTheDoctor_sfees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.DoctorFees.ItemPrice.DTOs
{
    public class DoctorFeesItemPriceDto
    {
        public int Id { get; private set; }
        public double DoctorFees { get; private set; }
        public UnitOfTheDoctorFeesDto UnitOfDoctorFees { get; private set; }
        public int UnitOfDoctorFeesId { get; private set; }
   
        public string EffectiveDateFrom { get; private set; }
        public string? EffectiveDateTo { get; private set; }
        public bool? IsDeleted { get; set; }

        public static DoctorFeesItemPriceDto FromDoctorFeesItemPrice(DoctorFeesItemPrice input) =>
        input is not null ? new DoctorFeesItemPriceDto
        {
            Id = input.Id,
            DoctorFees = input.DoctorFees,
            UnitOfDoctorFees = UnitOfTheDoctorFeesDto.FromUnitOfTheDoctorFees(input.UnitOfDoctorFees),
            EffectiveDateFrom = input.EffectiveDateFrom.ToString("yyyy-MM-dd"),
            EffectiveDateTo = input.EffectiveDateTo?.ToString("yyyy-MM-dd"),
            UnitOfDoctorFeesId = input.UnitOfDoctorFeesId,
            IsDeleted = input.IsDeleted
        } : null;
    }
}
