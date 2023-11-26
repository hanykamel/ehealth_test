using EHealth.ManageItemLists.Application.DoctorFees.ItemPrice.DTOs;
using EHealth.ManageItemLists.Application.Lookups.UnitOfTheDoctorFees.DTOs;
using EHealth.ManageItemLists.Domain.DoctorFees.UHIA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.PackageItems.DTOs
{
    public class GetAllDoctorsFeesDTO
    {
        public Guid Id { get; set; }
        public string? EHealthCode { get; set; }
        public string? NameEn { get; set; }
        public string? NameAr { get; set; }
        public double? DocotrFees { get; set; }
        public UnitOfTheDoctorFeesDto UnitOfTheDoctorFees{ get; set; }

        public static GetAllDoctorsFeesDTO FromGetAllDoctorsFees(DoctorFeesUHIA input) =>
            new GetAllDoctorsFeesDTO
            {
                Id = input.Id,
                EHealthCode = input.Code,
                NameAr = input.DescriptorAr,
                NameEn = input.DescriptorEn,
                DocotrFees = DoctorFeesItemPriceDto.FromDoctorFeesItemPrice(input.ItemListPrices.OrderByDescending(o=>o.EffectiveDateFrom).FirstOrDefault())?.DoctorFees,
                UnitOfTheDoctorFees = UnitOfTheDoctorFeesDto.FromUnitOfTheDoctorFees(input.ItemListPrices.OrderByDescending(o=>o.EffectiveDateFrom).FirstOrDefault()?.UnitOfDoctorFees)
            };
    }
}
