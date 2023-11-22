using EHealth.ManageItemLists.Application.DoctorFees.ItemPrice.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.PackageComplexityClassifications.DTOs;
using EHealth.ManageItemLists.Domain.DoctorFees.UHIA;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.DTOs
{
    public class DoctorFeesUHIADto
    {
        public Guid Id { get; private set; }
        public string EHealthCode { get; private set; }
        public string? DescriptorAr { get; private set; }
        public string DescriptorEn { get; private set; }
        public PackageComplexityClassificationDto PackageComplexityClassification { get; private set; }
        //public DateTime DataEffectiveDateFrom { get; private set; }
        //public DateTime? DataEffectiveDateTo { get; private set; }  
        public string DataEffectiveDateFrom { get; private set; }
        public string? DataEffectiveDateTo { get; private set; }
        public DoctorFeesItemPriceDto ItemListPrices { get; private set; } 
        public string? ModifiedBy { get; set; }
        public string? ModifiedOn { get; set; }
        public int ItemListId { get; set; }
        public bool? IsDeleted { get; set; }

        public static DoctorFeesUHIADto FromDoctorFeesUHIA(DoctorFeesUHIA input) =>
        new DoctorFeesUHIADto
        {
            Id = input.Id,
            EHealthCode = input.Code,
            DescriptorAr = input.DescriptorAr,
            DescriptorEn = input.DescriptorEn,
            PackageComplexityClassification = PackageComplexityClassificationDto.FromPackageComplexityClassification(input.PackageComplexityClassification),
            DataEffectiveDateFrom = input.DataEffectiveDateFrom.ToString("yyyy-MM-dd"),
            DataEffectiveDateTo = input.DataEffectiveDateTo?.ToString("yyyy-MM-dd"),
            ItemListPrices = DoctorFeesItemPriceDto.FromDoctorFeesItemPrice(input.ItemListPrices.OrderByDescending(e => e.EffectiveDateFrom).FirstOrDefault()),
            ModifiedBy = input.ModifiedBy,
            ModifiedOn = input.ModifiedOn?.ToString("yyyy-MM-dd"),
            ItemListId = input.ItemListId,
            IsDeleted = input.IsDeleted,
        };
    }
}
