using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Models.AccountViewModels;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.AppLicationDbContext.AccountingDatabase
{
    public class AccountingDbContext : DbContext
    {
        public AccountingDbContext(DbContextOptions<AccountingDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<JournalViewModel> Journals { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<CompanyViewModel> Company { get; set; }

    }
}