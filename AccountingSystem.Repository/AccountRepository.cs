using AccountingSystem.Abstractions.Repository;
using AccountingSystem.AppLicationDbContext.AccountingDatabase;
using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Models.AccountViewModels;
using Azure;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AccountingSystem.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountingDbContext _context;
        private readonly IConfiguration _DBCon;

        public AccountRepository(AccountingDbContext context, IConfiguration config) //: base(context)
        {
            _context = context;
            _DBCon = config;
        }
        public Users GetUsers(string userName, string password)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                // Use QueryFirstOrDefault instead of Query for getting a single result
                var result = _db.QueryFirstOrDefault<Users>("SELECT * FROM Users WHERE UName = @UName AND PWord = @PWord",
                    new { UName = userName, PWord = password });

                return result;
            }
        }


    }
}
