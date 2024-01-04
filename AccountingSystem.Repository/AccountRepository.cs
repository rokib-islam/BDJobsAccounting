using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


    }
}
