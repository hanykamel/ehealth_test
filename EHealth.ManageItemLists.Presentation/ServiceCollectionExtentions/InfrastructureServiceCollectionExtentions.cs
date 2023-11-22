
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Infrastructure.Repositories;
using EHealth.ManageItemLists.Infrastructure.Repositories.Lookups;
using EHealth.ManageItemLists.Presentation.Controllers;

namespace EHealth.ManageItemLists.Presentation.ServiceCollectionExtentions
{
    public static class InfrastructureServiceCollectionExtentions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddTransient<IServiceUHIARepository, ServiceUHIARepository>();
            services.AddTransient<IConsumablesAndDevicesUHIARepository, ConsumablesAndDevicesUHIARepository>();
            services.AddTransient<IItemListPriceRepository, ItemListPriceRepository>();
            services.AddTransient<IItemListRepository, ItemListRepository>();
            services.AddTransient<IItemListSubtypeRepository, ItemListSubtypeRepository>();
            services.AddTransient<IFacilityUHIARepository, FacilityUHIARepository>();
            services.AddTransient<IUnitRoomRepository, UnitRoomRepository>();
            services.AddTransient<IDevicesAndAssetsUHIARepository, DevicesAndAssetsUHIARepository>();
            services.AddTransient<IItemListTypeRepository, ItemListTypeRepository>();  
            services.AddTransient<ICategoriesRepository, CategoriesRepository>();
            services.AddTransient<ISubCategoriesRepository, SubCategoriesRepository>();
            services.AddTransient<IPriceUnitRepository, PriceUnitRepository>();
            services.AddTransient<IPriceUnitRepository, PriceUnitRepository>();
            services.AddTransient<IResourceUHIARepository, ResourceUHIARepository>();
            services.AddTransient<IPackageComplexityClassificationRepository, PackageComplexityClassificationRepository>();
            services.AddTransient<IUnitDOFRepository, UnitDOFRepository>();
            services.AddTransient<IDoctorFeesUHIARepository, DoctorFeesUHIARepository>();
            services.AddTransient<IConsumablesAndDevicesUHIARepository, ConsumablesAndDevicesUHIARepository>();
            services.AddTransient<IProcedureICHIRepository, ProcedureICHIRepository>();
            services.AddTransient<IDrugsUHIARepository, DrugsUHIARepository>();
            services.AddTransient<IUnitOfMeasureRepository, UnitOfMeasureRepository>();
            services.AddTransient<ILocalSpecialtyDepartmentsRepository, LocalSpecialtyDepartmentRepository>();
            services.AddTransient<IDrugsPackageTypeRepository, DrugsPackageTypeRepository>();
            services.AddTransient<IResourceItemPriceRepository, ResourceItemPriceRepository>();
            services.AddTransient<IUnitsTypeRepository, UnitsTypeRepository>();
            services.AddTransient<IRegistrationRepository, RegistrationRepository>();
            services.AddTransient<IReimbursementCategoryRepository, ReimbursementCategoryRepository>();
            services.AddTransient<IPackageSpecialtiesRepository, PackageSpecialtiesRepository>();
            services.AddTransient<IPackageTypeRepository, PackageTypeRepository>();
            services.AddTransient<IGlobelPackageTypeRepository, GlobalPackageTypeRepository>();
            services.AddTransient<IPackageSubTypeRepository, PackageSubTypeRepository>();
            services.AddTransient<IPackageHeaderRepository, PackageHeaderRepository>();

            return services;

        }
    }


}
