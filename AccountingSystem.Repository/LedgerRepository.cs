using AccountingSystem.Abstractions.Repository;
using AccountingSystem.AppLicationDbContext.AccountingDatabase;
using AccountingSystem.Models.AccountViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace AccountingSystem.Repository
{
    public class LedgerRepository : ILedgerRepository
    {
        private readonly AccountingDbContext _context;
        private readonly IConfiguration _DBCon;

        public LedgerRepository(AccountingDbContext context, IConfiguration config) //: base(context)
        {
            _context = context;
            _DBCon = config;
        }
        public async Task<List<ServiceViewModel>> GetService(int sTypy)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var result = await _db.QueryAsync<ServiceViewModel>(
                    "[dbo].[USP_GetService_List]", new { Type = sTypy },
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
        }
        public async Task<List<ServiceViewModel>> GetAllLedger(string isAdmin, string isAccount)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var result = await _db.QueryAsync<ServiceViewModel>(
                    "[dbo].[USP_LedgerList]",
                    new { UserAdmin = isAdmin, AccountsDep = isAccount },
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
        }


    }
}
