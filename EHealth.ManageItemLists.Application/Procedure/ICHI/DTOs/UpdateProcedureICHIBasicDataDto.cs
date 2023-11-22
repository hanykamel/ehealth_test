using EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI;

namespace EHealth.ManageItemLists.Application.Procedure.ICHI.DTOs
{
    public class UpdateProcedureICHIBasicDataDto
    {
        public Guid Id { get; set; }
        public string EHealthCode { get; set; }
        public string UHIAId { get; set; }
        public string? TitleAr { get; set; }
        public string TitleEn { get; set; }
        public int ServiceCategoryId { get; set; }
        public int ServiceSubCategoryId { get; set; }
        public int? ItemListId { get; set; }
        public DateTime DataEffectiveDateFrom { get; set; }
        public DateTime? DataEffectiveDateTo { get; set; }
        public int? LocalSpecialtyDepartmentId { get; set; }
        public ProcedureICHI ToProcedureICHI(string createdBy, string tenantId) => ProcedureICHI.Create(Id, EHealthCode, UHIAId, TitleAr, TitleEn,
               ServiceCategoryId, ServiceSubCategoryId,(int) ItemListId, DataEffectiveDateFrom, DataEffectiveDateTo, LocalSpecialtyDepartmentId, createdBy, tenantId);
    }
}
