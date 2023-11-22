using EHealth.ManageItemLists.Domain.DoctorFees.UHIA;
using EHealth.ManageItemLists.Domain.Sub_Categories;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.DTOs
{
    public class UpdateDoctoerFeesUHIABasicDataDto
    {
        public Guid Id { get; set; }
        public string EHealthCode { get; set; }
        public string? DescriptorAr { get; set; }
        public string DescriptorEn { get; set; }
        public int PackageCompexityClassificationId { get; set; }
        public DateTime DataEffectiveDateFrom { get; set; }
        public DateTime? DataEffectiveDateTo { get; set; }
        public int? ItemListId { get; set; }
        public DoctorFeesUHIA ToDrFeesUHIA(string createdBy, string tenantId) => DoctorFeesUHIA.Create(Id, EHealthCode, DescriptorAr, DescriptorEn,
            (int)ItemListId, PackageCompexityClassificationId,  DataEffectiveDateFrom, DataEffectiveDateTo, createdBy, tenantId);
    }
}
