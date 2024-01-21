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
        public async Task<List<LedgerListViewModel>> GetAllLedger(string isAdmin, string isAccount)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var result = await _db.QueryAsync<LedgerListViewModel>(
                    "[dbo].[USP_LedgerList]",
                    new { UserAdmin = isAdmin, AccountsDep = isAccount },
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
        }
        public async Task<int> GetOnlineLedgerId(string onlineProduct)
        {
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new
                    {
                        OnlineProduct = onlineProduct
                    };

                    // Assuming SERVICE_LIST is the name of the table
                    var result = await _db.QueryFirstOrDefaultAsync<int>("SELECT LedgerId FROM SERVICE_LIST WHERE ServiceName = @OnlineProduct", parameters);

                    // If no rows are found, result will be the default value for int, which is 0
                    return result;
                }
            }
            catch (Exception ex)
            {
                // Handle exception or log it
                throw ex;
            }
        }


    }
}
