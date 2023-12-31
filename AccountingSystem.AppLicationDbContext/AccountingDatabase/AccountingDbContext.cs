﻿using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Models.AccountViewModels;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.AppLicationDbContext.AccountingDatabase
{
    public class AccountingDbContext : DbContext
    {
        public AccountingDbContext(DbContextOptions<AccountingDbContext> options) : base(options)
        {
        }

        public DbSet<Journal> Journals { get; set; }
        
    }
}