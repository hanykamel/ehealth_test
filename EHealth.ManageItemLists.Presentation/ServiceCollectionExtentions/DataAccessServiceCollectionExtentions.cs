using EHealth.ManageItemLists.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace EHealth.ManageItemLists.Presentation.ServiceCollectionExtentions
{
    public static class DataAccessServiceCollectionExtentions
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EHealthDbContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("Default")).EnableSensitiveDataLogging(true).EnableDetailedErrors(true));
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            return services;
        }
    }
}
