using AccountingSystem.Abstractions.Repository;
using AccountingSystem.AppLicationDbContext.AccountingDatabase;
using AccountingSystem.Models.AccountDbModels;
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
        public async Task<Users> GetUsers(string userName, string password)
        {
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var result = await _db.QueryFirstOrDefaultAsync<Users>(
                        "SELECT * FROM Users WHERE UName = @UName AND PWord = @PWord",
                        new { UName = userName, PWord = password });

                    return result;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<Users>> GetSpecificUser()
        {
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var result = await _db.QueryAsync<Users>(
                        "SELECT UserID, Name FROM Users WHERE CanApprove = 0 AND AccountDep=1 AND AccessRight LIKE '%1%' ORDER BY Name;",
                        new { }
                    );

                    return result.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<List<Users>> GetApprovers()
        {
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var result = await _db.QueryAsync<Users>(
                        "SELECT UserID, Name FROM Users WHERE CanApprove=1 ORDER BY NAME",
                        new { }
                        );
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
