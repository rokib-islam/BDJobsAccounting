using AccountingSystem.Abstractions.Repository;
using AccountingSystem.AppLicationDbContext.AccountingDatabase;
using AccountingSystem.Models.AccountDbModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

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
        public async Task<Users> GetUsers(string userName, string password)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var result = await _db.QueryFirstOrDefaultAsync<Users>(
                    "SELECT * FROM Users WHERE UName = @UName AND PWord = @PWord",
                    new { UName = userName, PWord = password });

                return result;
            }
        }

        public async Task<List<Users>> GetSpecificUser()
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var result = await _db.QueryAsync<Users>(
                    "Select UserID, Name from Users where CanApprove = 0 and AccessRight like '%1%' order by name;",
                    new { }
                );

                return result.ToList();
            }
        }
    }
}
