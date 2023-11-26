using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.ConsumablesAndDevices;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.DoctorFees.ItemPrice;
using EHealth.ManageItemLists.Domain.DoctorFees.UHIA;
using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
using EHealth.ManageItemLists.Domain.DrugsPackageTypes;
using EHealth.ManageItemLists.Domain.DrugsPricing;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.GlobelPackageTypes;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.ItemListSubtypes;
using EHealth.ManageItemLists.Domain.ItemListTypes;
using EHealth.ManageItemLists.Domain.LocalSpecialtyDepartments;
using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
using EHealth.ManageItemLists.Domain.Locations;
using EHealth.ManageItemLists.Domain.PackageComplexityClassifications;
using EHealth.ManageItemLists.Domain.Packages.FeesOfResourcesPerUnitPackage.FeesOfResourcesPerUnitPackageComponents;
using EHealth.ManageItemLists.Domain.Packages.FeesOfResourcesPerUnitPackage.FeesOfResourcesPerUnitPackageResources;
using EHealth.ManageItemLists.Domain.Packages.FeesOfResourcesPerUnitPackage.FeesOfResourcesPerUnitPackageSummaries;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostDepreciationsAndMaintenances;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackagAssets;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackageComponents;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackageSummaries;
using EHealth.ManageItemLists.Domain.Packages.PackageHeaders;
using EHealth.ManageItemLists.Domain.Packages.SharedItemsPackages.SharedItemsPackageComponents;
using EHealth.ManageItemLists.Domain.Packages.SharedItemsPackages.SharedItemsPackageConsumablesAndDevices;
using EHealth.ManageItemLists.Domain.Packages.SharedItemsPackages.SharedItemsPackageDrugs;
using EHealth.ManageItemLists.Domain.PackageSpecialties;
using EHealth.ManageItemLists.Domain.PackageSubTypes;
using EHealth.ManageItemLists.Domain.PackageTypes;
using EHealth.ManageItemLists.Domain.PriceUnits;
using EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI;
using EHealth.ManageItemLists.Domain.RegistrationTypes;
using EHealth.ManageItemLists.Domain.ReimbursementCategories;
using EHealth.ManageItemLists.Domain.Resource.ItemPrice;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Sub_Categories;
using EHealth.ManageItemLists.Domain.UnitOfTheDoctor_sfees;
using EHealth.ManageItemLists.Domain.UnitRooms;
using EHealth.ManageItemLists.Domain.UnitsTypes;
using Microsoft.EntityFrameworkCore;

namespace EHealth.ManageItemLists.DataAccess
{
    public class EHealthDbContext : DbContext
    {
        public EHealthDbContext(DbContextOptions options) : base(options) { }
        public DbSet<ItemListType> ItemListTypes => Set<ItemListType>();
        public DbSet<ItemListSubtype> ItemListSubtypes => Set<ItemListSubtype>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<SubCategory> SubCategories => Set<SubCategory>();
        public DbSet<ItemListPrice> ItemListPrices => Set<ItemListPrice>();
        public DbSet<ItemList> ItemLists => Set<ItemList>();
        public DbSet<ServiceUHIA> ServicesUHIA => Set<ServiceUHIA>();
        public DbSet<ConsumablesAndDevicesUHIA> ConsumablesAndDevicesUHIA => Set<ConsumablesAndDevicesUHIA>();
        public DbSet<UnitOfMeasure> UnitOfMeasures => Set<UnitOfMeasure>();
        public DbSet<LocalSpecialtyDepartment> LocalSpecialtyDepartments => Set<LocalSpecialtyDepartment>();
        public DbSet<ProcedureICHI> ProceduresICHI => Set<ProcedureICHI>();
        public DbSet<UnitRoom> UnitRooms => Set<UnitRoom>();
        public DbSet<DevicesAndAssetsUHIA> DevicesAndAssetsUHIA => Set<DevicesAndAssetsUHIA>();
        public DbSet<FacilityUHIA> FacilityUHIA => Set<FacilityUHIA>();
        public DbSet<PriceUnit> PriceUnits => Set<PriceUnit>();
        public DbSet<ResourceItemPrice> ResourceItemPrices => Set<ResourceItemPrice>();
        public DbSet<ResourceUHIA> ResourceUHIA => Set<ResourceUHIA>();
        public DbSet<DrugUHIA> DrugsUHIA => Set<DrugUHIA>();
        public DbSet<RegistrationType> RegistrationTypes => Set<RegistrationType>();
        public DbSet<DrugsPackageType> DrugsPackageTypes => Set<DrugsPackageType>();
        public DbSet<UnitsType> UnitsTypes => Set<UnitsType>();
        public DbSet<ReimbursementCategory> ReimbursementCategories => Set<ReimbursementCategory>();
        public DbSet<DrugPrice> DrugPrices => Set<DrugPrice>();
        public DbSet<UnitDOF> UnitsOfTheDoctorFees => Set<UnitDOF>();
        public DbSet<DoctorFeesItemPrice> DoctorFeesItemPrices => Set<DoctorFeesItemPrice>();
        public DbSet<PackageComplexityClassification> PackageComplexityClassifications => Set<PackageComplexityClassification>();
        public DbSet<DoctorFeesUHIA> DoctorFeesUHIA => Set<DoctorFeesUHIA>();
        public DbSet<PackageType> PackageTypes => Set<PackageType>();
        public DbSet<PackageSubType> PackageSubTypes => Set<PackageSubType>();
        public DbSet<GlobelPackageType> GlobelPackageTypes => Set<GlobelPackageType>();
        public DbSet<PackageSpecialty> PackageSpecialties => Set<PackageSpecialty>();
        public DbSet<PackageHeader> PackageHeaders => Set<PackageHeader>();
        public DbSet<InvestmentCostPackageComponent> InvestmentCostPackageComponents => Set<InvestmentCostPackageComponent>();
        public DbSet<InvestmentCostPackageAsset> InvestmentCostPackageAssets => Set<InvestmentCostPackageAsset>();
        public DbSet<InvestmentCostDepreciationAndMaintenance> InvestmentCostDepreciationsAndMaintenances => Set<InvestmentCostDepreciationAndMaintenance>();
        public DbSet<FeesOfResourcesPerUnitPackageResource> FeesOfResourcesPerUnitPackageResources => Set<FeesOfResourcesPerUnitPackageResource>();
        public DbSet<FeesOfResourcesPerUnitPackageSummary> FeesOfResourcesPerUnitPackageSummaries => Set<FeesOfResourcesPerUnitPackageSummary>();
        public DbSet<FeesOfResourcesPerUnitPackageComponent> FeesOfResourcesPerUnitPackageComponents => Set<FeesOfResourcesPerUnitPackageComponent>();
        public DbSet<Location> Locations => Set<Location>();
        public DbSet<SharedItemsPackageDrug> SharedItemsPackageDrugs => Set<SharedItemsPackageDrug>();
        public DbSet<SharedItemsPackageConsumableAndDevice> SharedItemsPackageConsumableAndDevices => Set<SharedItemsPackageConsumableAndDevice>();
        public DbSet<SharedItemsPackageComponent> SharedItemsPackageComponents => Set<SharedItemsPackageComponent>();
        public DbSet<InvestmentCostPackageSummary> InvestmentCostPackageSummaries => Set<InvestmentCostPackageSummary>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
