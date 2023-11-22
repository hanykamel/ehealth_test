using EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Procedure.ICHI.DTOs
{
    public class CreateProcedureICHIBasicDataDto
    {
        public string EHealthCode { get; set; }
        public string UHIAId { get; set; }
        public string? TitleAr { get; set; }
        public string TitleEn { get; set; }
        public int ServiceCategoryId { get; set; }
        public int ServiceSubCategoryId { get; set; }
        public int ItemListId { get; set; }
        public DateTime DataEffectiveDateFrom { get; set; }
        public DateTime? DataEffectiveDateTo { get; set; }
        public int? LocalSpecialtyDepartmentId { get; set; }

        public ProcedureICHI ToProcedureICHI(string createdBy, string tenantId) => ProcedureICHI.Create(null, EHealthCode, UHIAId, TitleAr, TitleEn,
                ServiceCategoryId, ServiceSubCategoryId, ItemListId, DataEffectiveDateFrom, DataEffectiveDateTo, LocalSpecialtyDepartmentId, createdBy, tenantId);
    }
}
    

