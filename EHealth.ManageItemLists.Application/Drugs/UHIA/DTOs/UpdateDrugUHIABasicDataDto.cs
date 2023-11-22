using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
using EHealth.ManageItemLists.Domain.Sub_Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.DTOs
{
    public class UpdateDrugUHIABasicDataDto
    {
        public Guid Id { get; set; }
        public string? EHealthCode { get; set; }
        public string LocalDrugCode { get; set; }
        public string? InternationalNonProprietaryName { get; set; }
        public string ProprietaryName { get; set; }
        public string DosageForm { get; set; }
        public string? RouteOfAdministration { get; set; }
        public string? Manufacturer { get; set; }
        public string? MarketAuthorizationHolder { get; set; }
        public int? RegistrationTypeId { get; set; }
        public int? DrugsPackageTypeId { get; set; }
        public int? MainUnitId { get; set; }
        public int? NumberOfMainUnit { get; set; }
        public int SubUnitId { get; set; }
        public int? NumberOfSubunitPerMainUnit { get; set; }
        public int? TotalNumberSubunitsOfPack { get; set; }
        public int? ReimbursementCategoryId { get; set; }
        public DateTime DataEffectiveDateFrom { get; set; }
        public DateTime? DataEffectiveDateTo { get; set; }
        public int? ItemListId { get; set; }
        public DrugUHIA ToDrugsUHIA(string createdBy, string tenantId) => DrugUHIA.Create(Id, (int)ItemListId, EHealthCode, LocalDrugCode, InternationalNonProprietaryName,
                ProprietaryName, DosageForm, RouteOfAdministration, Manufacturer, MarketAuthorizationHolder, RegistrationTypeId == 0 ? null : RegistrationTypeId, DrugsPackageTypeId == 0 ? null : DrugsPackageTypeId, MainUnitId == 0 ? null : MainUnitId,
                NumberOfMainUnit, SubUnitId, NumberOfSubunitPerMainUnit, TotalNumberSubunitsOfPack, ReimbursementCategoryId, DataEffectiveDateFrom, DataEffectiveDateTo, createdBy, tenantId);
    }
}
