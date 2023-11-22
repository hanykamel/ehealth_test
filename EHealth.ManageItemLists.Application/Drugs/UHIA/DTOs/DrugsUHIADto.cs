using EHealth.ManageItemLists.Application.Lookups.DrugsPackageTypes.DTOs;
using EHealth.ManageItemLists.Application.Lookups.PackageType.DTOs;
using EHealth.ManageItemLists.Application.Lookups.RegistrationTypes.DTOs;
using EHealth.ManageItemLists.Application.Lookups.ReimbursementCategories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.UnitsTypes.DTOs;
using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
using EHealth.ManageItemLists.Domain.DrugsPackageTypes;
using EHealth.ManageItemLists.Domain.DrugsPricing;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.RegistrationTypes;
using EHealth.ManageItemLists.Domain.ReimbursementCategories;
using EHealth.ManageItemLists.Domain.UnitsTypes;

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.DTOs
{
    public class DrugsUHIADto
    {
        public Guid Id { get; set; }
        public int ItemListId { get; set; }
        public string? EHealthCode { get; private set; }
        public string? LocalDrugCode { get; private set; }
        public string? InternationalNonProprietaryName { get; private set; }
        public string ProprietaryName { get; private set; }
        public string DosageForm { get; private set; }
        public string Manufacturer { get; private set; }
        public string MarketAuthorizationHolder { get; private set; }
        public string? RouteOfAdministration { get; private set; }
        public int? DrugsPackageTypeId { get; private set; }
        //public PackageTypeDto? DrugsPackageType { get; private set; }


        public int? RegistrationTypeId { get; private set; }
        //public RegistrationType? RegistrationType { get; private set; }
        public UnitsTypeDto? MainUnit { get; private set; }
        public int? NumberOfMainUnit { get; private set; }
        public int? SubUnitId { get; private set; }
        //public UnitsTypeDto? SubUnit { get; private set; }
        public int? NumberOfSubunitPerMainUnit { get; private set; }
        public int? TotalNumberSubunitsOfPack { get; private set; }
        public int? ReimbursementCategoryId { get; private set; }
        public ReimbursementCategoryDto? ReimbursementCategory { get; private set; }
        //public DateTime DataEffectiveDateFrom { get; private set; }
        //public DateTime? DataEffectiveDateTo { get; private set; }
        public string DataEffectiveDateFrom { get; private set; }
        public string? DataEffectiveDateTo { get; private set; }
        public DrugPriceDto DrugPrice { get; private set; }


        public RegistrationTypeDto? RegistrationType { get; private set; }
        public UnitsTypeDto? MainType { get; private set; }
        public UnitsTypeDto? SubUnit { get; private set; }
        //public ReimbursementCategoryDto? ReimbursementCategory { get; private set; }
        public DrugsPackageTypesDto? DrugsPackageType { get; private set; }
        public bool IsDeleted { get; set; }

        public static DrugsUHIADto FromDrugsUHIA(DrugUHIA input) =>
      new DrugsUHIADto
      {
          Id = input.Id,
          ItemListId = input.ItemListId,
          EHealthCode = input.EHealthDrugCode,
          LocalDrugCode = input.LocalDrugCode,
          InternationalNonProprietaryName = input.InternationalNonProprietaryName,
          ProprietaryName = input.ProprietaryName,
          DosageForm = input.DosageForm,
          Manufacturer = input.Manufacturer,
          MarketAuthorizationHolder = input.MarketAuthorizationHolder,
          RouteOfAdministration = input.RouteOfAdministration,
          DrugsPackageTypeId = input.DrugsPackageTypeId,

          ////DrugsPackageType = PackageTypeDto.FromPackageType(input.DrugsPackageType),
          DataEffectiveDateFrom = input.DataEffectiveDateFrom.ToString("yyyy-MM-dd"),
          DataEffectiveDateTo = input.DataEffectiveDateTo?.ToString("yyyy-MM-dd"),
          MainUnit = UnitsTypeDto.FromUnitsType(input.MainUnit),
          //MainUnitId = input.MainUnitId,inp
          DrugsPackageType = DrugsPackageTypesDto.FromDrugsPackageType(input.DrugsPackageType),
          NumberOfMainUnit = input.NumberOfMainUnit,
          NumberOfSubunitPerMainUnit = input.NumberOfSubunitPerMainUnit,
          RegistrationTypeId = input.RegistrationTypeId,
          RegistrationType = RegistrationTypeDto.FromRegistrationType(input.RegistrationType),
          ReimbursementCategory = ReimbursementCategoryDto.FromReimbursementCategory(input.ReimbursementCategory),
          ReimbursementCategoryId = input.ReimbursementCategoryId,
          SubUnit = UnitsTypeDto.FromUnitsType(input.SubUnit),
          SubUnitId = input.SubUnitId,
          TotalNumberSubunitsOfPack = input.TotalNumberSubunitsOfPack,
          DrugPrice = DrugPriceDto.FromDrugPrice(input.DrugPrices.OrderByDescending(e => e.EffectiveDateFrom).FirstOrDefault()),
          IsDeleted = input.IsDeleted,
      };
    }
}
