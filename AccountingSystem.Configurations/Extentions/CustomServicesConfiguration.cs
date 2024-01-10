using AccountingSystem.Models.AccountSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccountingSystem.Configurations.Extentions
{
    public static class CustomServicesConfiguration
    {
        public static IServiceCollection AddCustomConfigure(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<ConnectionStringList>(config.GetSection("ConnectionStrings"));
            return services;
        }
    }
}