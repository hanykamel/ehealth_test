using EHealth.ManageItemLists.Domain.Resource.UHIA;

namespace EHealth.ManageItemLists.Application.Resource.UHIA.DTOs
{
    public class UpdateResourceUHIABasicDataDto
    {
        public Guid Id { get; set; }
        public string EHealthCode { get; set; }
        public string? DescriptorAr { get; set; }
        public string DescriptorEn { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public DateTime DataEffectiveDateFrom { get; set; }
        public DateTime? DataEffectiveDateTo { get; set; }
        public int? ItemListId { get; set; }
        public ResourceUHIA ToResourceUHIA(string createdBy, string tenantId) => ResourceUHIA.Create(Id,EHealthCode, DescriptorAr, DescriptorEn
              , CategoryId, SubCategoryId, DataEffectiveDateFrom,(int) ItemListId, DataEffectiveDateTo, createdBy, tenantId);
    }
}
