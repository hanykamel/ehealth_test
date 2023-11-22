using DocumentFormat.OpenXml.Office2010.Excel;
using EHealth.ManageItemLists.Domain.DoctorFees.UHIA;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.DTOs
{
    public class CreateDoctorFeesUHIABasicDataDto
    {
        public string EHealthCode { get; set; }
        public string? DescriptorAr { get; set; }
        public string DescriptorEn { get; set; }
        public int ItemListId { get; set; }
        public int PackageCompexityClassificationId { get; set; }
        public DateTime DataEffectiveDateFrom { get; set; }
        public DateTime? DataEffectiveDateTo { get; set; }
        public DoctorFeesUHIA ToDrFeesUHIA(string createdBy, string tenantId) => DoctorFeesUHIA.Create(null,EHealthCode, DescriptorAr, DescriptorEn,
                ItemListId,PackageCompexityClassificationId, DataEffectiveDateFrom, DataEffectiveDateTo, createdBy, tenantId);

    }
}
