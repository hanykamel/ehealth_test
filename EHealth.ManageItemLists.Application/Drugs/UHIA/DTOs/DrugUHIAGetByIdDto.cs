using EHealth.ManageItemLists.Application.DoctorFees.ItemPrice.DTOs;
using EHealth.ManageItemLists.Application.Lookups.DrugsPackageTypes.DTOs;
using EHealth.ManageItemLists.Application.Lookups.PackageType.DTOs;
using EHealth.ManageItemLists.Application.Lookups.RegistrationTypes.DTOs;
using EHealth.ManageItemLists.Application.Lookups.ReimbursementCategories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.UnitsTypes.DTOs;
using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
using EHealth.ManageItemLists.Domain.ReimbursementCategories;

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.DTOs
{
    public class DrugUHIAGetByIdDto
    {
        public Guid Id { get; set; }
        public string? EHealthCode { get; private set; }
        public string? LocalDrugCode { get; private set; }
        public string? InternationalNonProprietaryName { get; private set; }
        public string ProprietaryName { get; private set; }
        public string DosageForm { get; private set; }
        public string? RouteOfAdministration { get; private set; }
        public string? Manufacturer { get; private set; }
        public string? MarketAuthorizationHolder { get; private set; }
        public int? RegistrationTypeId { get; private set; }
        public RegistrationTypeDto? RegistrationType { get; private set; }
        public int? DrugsPackageTypeId { get; private set; }
        public DrugsPackageTypesDto? DrugsPackageType { get; private set; }
        public int? MainUnitId { get; private set; }
        public UnitsTypeDto? MainUnit { get; private set; }
        public int? NumberOfMainUnit { get; private set; }
        public int? SubUnitId { get; private set; }
        public UnitsTypeDto? SubUnit { get; private set; }
        public int? NumberOfSubunitPerMainUnit { get; private set; }
        public int? TotalNumberSubunitsOfPack { get; private set; }
        public int? ReimbursementCategoryId { get; private set; }
        public ReimbursementCategoryDto? ReimbursementCategory { get; private set; }
        public string DataEffectiveDateFrom { get; private set; }
        public string? DataEffectiveDateTo { get; private set; }
        public IList<GetDrugPriceDto> DrugPrices { get; private set; } = new List<GetDrugPriceDto>();
        public bool IsDeleted { get; set; }


        public static DrugUHIAGetByIdDto FromDrugsGetById(DrugUHIA input) =>
        new DrugUHIAGetByIdDto
        {
            Id = input.Id,
            EHealthCode = input.EHealthDrugCode,
            LocalDrugCode = input.LocalDrugCode,
            InternationalNonProprietaryName = input.InternationalNonProprietaryName,
            ProprietaryName = input.ProprietaryName,
            DosageForm = input.DosageForm,
            RouteOfAdministration = input.RouteOfAdministration,
            Manufacturer = input.Manufacturer,
            MarketAuthorizationHolder = input.MarketAuthorizationHolder,
            RegistrationTypeId = input.RegistrationTypeId,
            RegistrationType = RegistrationTypeDto.FromRegistrationType(input.RegistrationType),
            DrugsPackageTypeId = input.DrugsPackageTypeId,
            DrugsPackageType = DrugsPackageTypesDto.FromDrugsPackageType(input.DrugsPackageType),
            MainUnitId = input.MainUnitId,
            MainUnit = UnitsTypeDto.FromUnitsType(input.MainUnit),
            NumberOfMainUnit = input.NumberOfMainUnit,
            SubUnitId = input.SubUnitId,
            SubUnit = UnitsTypeDto.FromUnitsType(input.SubUnit),
            NumberOfSubunitPerMainUnit = input.NumberOfSubunitPerMainUnit,
            TotalNumberSubunitsOfPack = input.TotalNumberSubunitsOfPack,
            ReimbursementCategoryId = input.ReimbursementCategoryId,
            ReimbursementCategory = ReimbursementCategoryDto.FromReimbursementCategory(input.ReimbursementCategory),
            DataEffectiveDateFrom = input.DataEffectiveDateFrom.ToString("yyyy-MM-dd"),
            DataEffectiveDateTo = input.DataEffectiveDateTo?.ToString("yyyy-MM-dd"),
            DrugPrices= input.DrugPrices.Select(p => GetDrugPriceDto.FromDrugPriceDto(p)).ToList() ,
            IsDeleted = input.IsDeleted

        };
    }
    }
