using AccountingSystem.Abstractions.Repository;
using AccountingSystem.AppLicationDbContext.AccountingDatabase;
using AccountingSystem.Models.AccountDbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AccountingSystem.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountingDbContext _context;
        private readonly IConfiguration _configaration;

        public AccountRepository(AccountingDbContext context, IConfiguration config) //: base(context)
        {
            _context = context;
            _configaration = config;
        }
        public async Task<List<Users>> GetUsers(string userName, string password)
        {

            var blogs = await _context.Users
                .Where(b => b.UName == userName && b.PWord == password)
                .ToListAsync();

            return blogs;


        }

    }
}
