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
        public async Task<List<Ledger>> GetProducts(int admin, int account, string groupname, string isAll, string isI, int isVatType)
        {
            var ledgers = new List<Ledger>();

            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new
                    {
                        UserAdmin = admin,
                        AccountsDep = account,
                        MGroup = groupname,
                        All = isAll,
                        InvoiceLedger = isI,
                        Tax = isVatType
                    };

                    var result = await _db.QueryAsync<Ledger>("USP_LedgerList", parameters, commandType: CommandType.StoredProcedure);

                    ledgers = result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ledgers;
        }
        public async Task<List<Ledger>> GetLedgersWithBalance()
        {
            var ledgers = new List<Ledger>();

            try
            {
                string sqlQuery = "SELECT id, sbname as GroupName, FORMAT(balance, '##,##0.00 '+account) As Account FROM Ledger WHERE (LedgerAcc = 1 or under like '3,1075%') ORDER BY sbname";

                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var result = await _db.QueryAsync<Ledger>(sqlQuery);
                    ledgers.AddRange(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ledgers;
        }



    }
}
