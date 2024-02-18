using AccountingSystem.Abstractions.Repository;
using AccountingSystem.AppLicationDbContext.AccountingDatabase;
using AccountingSystem.Models.AccountViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace AccountingSystem.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly AccountingDbContext _context;
        private readonly IConfiguration _DBCon;

        public ReportRepository(AccountingDbContext context, IConfiguration config) //: base(context)
        {
            _context = context;
            _DBCon = config;
        }
        public async Task<List<InvoiceReport>> GetInvoiceReportAsync(string invoiceNo)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@InvoiceNo", invoiceNo);

                var invoices = await _db.QueryAsync<InvoiceReport>("USP_Show_Invoice", parameters,
                    commandType: CommandType.StoredProcedure);

                return invoices.AsList();
            }
        }
        public async Task<List<ChalanReport>> GetChalanReportNew(string invoiceNo)
        {
            var chalans = new List<ChalanReport>();

            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var query = "USP_Show_Challan_New";
                    var parameters = new { InvoiceNo = invoiceNo };

                    var result = await _db.QueryAsync<ChalanReport>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);

                    chalans.AddRange(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return chalans;
        }
        public async Task<List<Ledger>> GetTrialBalanceReportAsync(string type, string startingDate, string endDate)
        {

            var ledgers = new List<Ledger>();

            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new { Type = type, StartingDate = startingDate, EndDate = endDate };
                    var result = await _db.QueryAsync<Ledger>("USP_TRIAL_BALANCE_RPT", parameters, commandType: CommandType.StoredProcedure);

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
