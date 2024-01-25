using AccountingSystem.Models.AccountDbModels;
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
            modelBuilder.Entity<Users>().HasKey(u => u.UserID);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Journal> Journals { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Company> Company { get; set; }

    }
}