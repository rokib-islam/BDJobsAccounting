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
        public async Task<List<TrialBalanceRptModel>> GetTrialBalanceReportAsync(string type, string startingDate, string endDate)
        {

            var ledgers = new List<TrialBalanceRptModel>();

            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new { Type = type, StartingDate = startingDate, EndDate = endDate };
                    var result = await _db.QueryAsync<TrialBalanceRptModel>("USP_TRIAL_BALANCE_RPT", parameters, commandType: CommandType.StoredProcedure);

                    ledgers.AddRange(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ledgers;
        }

        public async Task<List<LabelReport>> GetLabelReport(string type, string list)
        {
            var labels = new List<LabelReport>();

            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {

                    var result = await _db.QueryAsync<LabelReport>("USP_LIST_OF_LABEL_RPT", new { LabelType = type, listIds = list }, commandType: System.Data.CommandType.StoredProcedure);
                    labels.AddRange(result);
                }
            }
            catch (Exception ex)
            {
                // Handle exception appropriately, maybe log it.
                throw;
            }

            return labels;
        }

        public async Task<List<LoadVatTaxCollectionDataModel_Response>> LoadVatTaxCollectionData(LoadVatTaxCollectionDataModel_Request model)
        {
            try
            {
                var parameters = new
                {
                    CId = model.CId,
                    Fromdate = model.FromDate,
                    Todate = model.ToDate,
                };
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var result = await _db.QueryAsync<LoadVatTaxCollectionDataModel_Response>("USP_LoadTaxVatCollectionData", parameters, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<List<JournalVoucherReport>> GetVoucherReportAsync(int Jid)
        {
            var vouchers = new List<JournalVoucherReport>();

            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {

                    var sqlQuery = "USP_JOURNAL_VOUCHER_RPT @JID";
                    var parameters = new { JID = Jid };

                    var result = await _db.QueryAsync<JournalVoucherReport>(sqlQuery, parameters, commandType: CommandType.StoredProcedure);

                    vouchers = result.AsList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return vouchers;
        }

    }
}
