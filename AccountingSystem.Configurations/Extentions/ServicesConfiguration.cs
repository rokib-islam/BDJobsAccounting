using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Abstractions.Repository;
using AccountingSystem.BLL;
using AccountingSystem.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AccountingSystem.Configurations.Extentions
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            #region Account
            services.AddScoped<IAccountManager, AccountManager>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            #endregion 

            #region Journal
            services.AddScoped<IJournalManager, JournalManager>();
            services.AddScoped<IJournalRepository, JournalRepository>();
            #endregion

            #region Company
            services.AddScoped<ICompanyManager, CompanyManager>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            #endregion

            #region Ledger
            services.AddScoped<ILedgerManager, LedgerManager>();
            services.AddScoped<ILedgerRepository, LedgerRepository>();
            #endregion



            services.AddScoped<DbContext>();
            return services;
        }
    }
}