using AccountingSystem.Abstractions.Repository;
using AccountingSystem.AppLicationDbContext.AccountingDatabase;
using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Models.AccountViewModels;
using Azure;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        public async Task<List<LedgerListViewModel>> GetAllEveryLedger(string isCashCollection)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var result = await _db.QueryAsync<LedgerListViewModel>(
                    "[dbo].[USP_LedgerList]",
                    new { IsCashCollection = isCashCollection },
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
        public async Task<List<LedgerViewModel>> GetProducts(int admin, int account, string groupname, string isAll, string isI, int isVatType)
        {
            var ledgers = new List<LedgerViewModel>();

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

                    var result = await _db.QueryAsync<LedgerViewModel>("USP_LedgerList", parameters, commandType: CommandType.StoredProcedure);

                    ledgers = result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ledgers;
        }
        public async Task<List<LedgerViewModel>> GetLedgersWithBalance()
        {
            var ledgers = new List<LedgerViewModel>();

            try
            {
                string sqlQuery = "SELECT id, sbname as GroupName, FORMAT(balance, '##,##0.00 '+account) As Account FROM Ledger WHERE (LedgerAcc = 1 or under like '3,1075%') ORDER BY sbname";

                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var result = await _db.QueryAsync<LedgerViewModel>(sqlQuery);
                    ledgers.AddRange(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ledgers;
        }

        public async Task<List<LedgerViewModel>> GetLedgerListByKey(string key)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var query = "SELECT Id, SBName FROM Ledger WHERE LedgerAcc = 1 and SBName like @Key ORDER BY SBName";
                var parameters = new { Key = "%" + key + "%" };

                var result = await _db.QueryAsync<LedgerViewModel>(query, parameters);
                return result.ToList();
            }
        }

        public async Task<List<LedgerViewModel>> GetAllLedgers()
        {
            var ledgers = new List<LedgerViewModel>();
            try
            {
                string sqlQuery = "SELECT Id, SBName AS GroupName, Under, MGroup AS MaingroupName, LevelNo, " +
                    "OpeningBalance, LedgerAcc AS IsLedgerAccount, Balance, Account, OpeningDate, ServiceID FROM Ledger";

                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {

                    ledgers = (await _db.QueryAsync<LedgerViewModel>(sqlQuery)).AsList();
                }

                return ledgers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task SaveLedgerAsync(LedgerViewModel aLedger)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                await _db.ExecuteAsync(
                "INSERT INTO dbo.Ledger(SBName, Under, MGroup, LevelNo, LedgerAcc, ServiceID) " +
                "VALUES(@GroupName, @Under, @MaingroupName, @LevelNo, @LedgerAcc, @ServiceID)",
                new
                {
                    aLedger.GroupName,
                    aLedger.Under,
                    aLedger.MaingroupName,
                    aLedger.LevelNo,
                    aLedger.LedgerAcc,
                    aLedger.ServiceID
                });
            }

        }
        public async Task<int> UpdateLedgerAsync(LedgerViewModel aLedger)
        {
            var sqlQuery = @"
                            UPDATE dbo.Ledger 
                            SET SBName = @SBName, 
                                Under = @Under, 
                                MGroup = @MGroup, 
                                LevelNo = @LevelNo, 
                                LedgerAcc = @LedgerAcc, 
                                ServiceID = @ServiceID 
                            WHERE Id = @Id";

            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var result = await _db.ExecuteAsync(sqlQuery, new
                {
                    SBName = aLedger.SBName, // corrected property name
                    Under = aLedger.Under,
                    MGroup = aLedger.MGroup, // corrected property name
                    LevelNo = aLedger.LevelNo,
                    LedgerAcc = aLedger.LedgerAcc,
                    ServiceID = aLedger.ServiceID,
                    Id = aLedger.Id
                });

                return result; // Return the number of rows affected
            }
        }
        public async Task<string> DeleteLedgerAsync(int ledgerId)
        {
            var sql = "DELETE FROM dbo.Ledger WHERE Id = @LedgerId";


            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    await _db.ExecuteAsync(sql, new { LedgerId = ledgerId });
                    return "Ledger deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                return $"Error deleting ledger: {ex.Message}";
            }
        }


        //public async Task<List<LedgerViewModel>> GetProductListByKey(string Key)
        //{
        //    using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
        //    {
        //        var query = "SELECT id, SBName As LadgerName FROM ledger WHERE ledgerAcc=1 and MGroup='Revenue' and SBName LIKE @Key ORDER BY SBName";
        //        var parameters = new { Key = "%" + Key + "%" };

        //        var result = await _db.QueryAsync<LedgerViewModel>(query, parameters);
        //        return result.ToList();
        //    }

        //}

        public async Task<List<LedgerViewModel>> GetProductList()
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var query = "SELECT Id, SBName As LadgerName, s.VatRate, s.UnitPrice FROM Ledger l INNER JOIN SevicewiseVatRate s on s.ServiceId = l.ServiceID WHERE ledgerAcc=1 and MGroup='Revenue' and l.ServiceID IS NOT NULL ORDER BY SBName";
                var parameters = new { };

                var result = await _db.QueryAsync<LedgerViewModel>(query, parameters);
                return result.ToList();
            }

        }

        public async Task<List<LedgerViewModel>> GetProductById(int pId)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                //var query = "SELECT Id, SBName FROM Ledger WHERE Id=@PId";
                var query = "SELECT Id, SBName As LadgerName, s.VatRate FROM Ledger l INNER JOIN SevicewiseVatRate s on s.ServiceId = l.ServiceID WHERE Id=@PId";
                var parameters = new { PId = pId };

                var result = await _db.QueryAsync<LedgerViewModel>(query, parameters);
                return result.ToList();
            }

        }

        
        public async Task<List<LoadServiceListModel>> LoadServiceList(LoadServiceListModel model)
        {
            try
            {
                var parameters = new
                {
                    PageNo = model.PageNo,
                    PageSize = model.PageSize,
                    Id = model.Id,
                    ServiceName = model.ServiceName,
                    VatRate = model.VatRate,
                    UnitPrice = model.UnitPrice,
                    Type = model.Type,
                };
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var result = await _db.QueryAsync<LoadServiceListModel>("USP_VIEW_ADD_SERVICE", parameters, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<LedgerViewModel>> GetStaffPFIAccountList()
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var query = "SELECT * FROM Ledger WHERE Under LIKE '%15761%'";
                var parameters = new { };

                var result = await _db.QueryAsync<LedgerViewModel>(query, parameters);
                return result.ToList();
            }

        }

        public async Task<List<LedgerViewModel>> GetLedgerName()
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var query = "SELECT * FROM Ledger WHERE MGroup = 'Expense'";
                var parameters = new { };

                var result = await _db.QueryAsync<LedgerViewModel>(query, parameters);
                return result.ToList();
            }

        }
    }




}
