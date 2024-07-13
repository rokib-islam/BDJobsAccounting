using AccountingSystem.Abstractions.Repository;
using AccountingSystem.AppLicationDbContext.AccountingDatabase;
using AccountingSystem.Models.AccountViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Security.Cryptography;

namespace AccountingSystem.Repository
{
    public class JournalRepository : IJournalRepository
    {
        private readonly AccountingDbContext _context;
        private readonly IConfiguration _DBCon;

        public JournalRepository(AccountingDbContext context, IConfiguration dbcon) //: base(context)
        {
            _context = context;
            _DBCon = dbcon;
        }

        public async Task<List<JouralView>> GetJournalListAsync(int pageNo, int pageSize, int isPreview, string dateType, string startDate, string endDate, int LedgerId, string ledgerName, int companyId, int approvedBy, int postedBy, int isApproved)
        {
            try
            {
                var parameters = new
                {
                    PageNo = pageNo,
                    PageSize = pageSize,
                    Preview = isPreview,
                    DateType = dateType,
                    StartDate = startDate,
                    EndDate = endDate,
                    LedgerID = LedgerId,
                    LedgerName = ledgerName,
                    CompanyID = companyId,
                    ApprovedBy = approvedBy,
                    PostedBy = postedBy,
                    Approved = isApproved
                };

                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var result = await _db.QueryAsync<JouralView>("USP_VIEW_JOURNAL_LIST", parameters, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<object> GetJournalsForTrialBalance(string pageNo, string pageSize, string tno, string fromDate, string endDate)
        {   
            try
            {

                var parameters = new
                {
                    PageNo = pageNo,
                    PageSize = pageSize,
                    TNO = tno,
                    FromDate = fromDate,
                    EndDate = endDate,
                };

                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var result = await _db.QueryAsync<JournalViewModel>("USP_VIEW_SALE_WISE_JOURNAL_LIST", parameters, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<string> GetClosingDateAsync()
        {
            string date = "";

            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {

                    date = await _db.QueryFirstOrDefaultAsync<string>("SELECT ClosingDate FROM CloseingDateInfo");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return date ?? "";
        }
        public async Task<string> UpdateSalesJournalAsync(UpdateSalesJournal updateInfo)
        {
            var result = "";
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new
                    {
                        SID = updateInfo.Sid,
                        VATID = updateInfo.VatId,
                        TNO = updateInfo.Tno,
                        ODuration = updateInfo.OldDuration,
                        CDuration = updateInfo.NewDuration,
                        OAmount = updateInfo.OldAmount,
                        CAmount = updateInfo.NewAmount,
                        OAmountVAT = updateInfo.OldAmount,
                        CAmountVAT = updateInfo.NewAmount,
                        JDate = updateInfo.FromDate,
                        Description = updateInfo.Description,
                        UserID = updateInfo.UserId
                    };

                    await _db.ExecuteAsync("USP_SALES_JOURNAL_UPDATE_D", parameters, commandType: CommandType.StoredProcedure);
                    result = "Success";
                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        public async Task<JournalViewModel> GetJournalBySIdAsync(int sId)
        {
            var sql = "SELECT Id, Jid, Sid, Description, Debt, Credit, AccType, JDate, Tno, Notify, PostDate, Lock, UserID, ApprovedBy, ApprovalDate, UpdatedDate, UpdatedBy FROM Journal WHERE Sid = @Sid";

            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var result = await _db.QuerySingleOrDefaultAsync<JournalViewModel>(sql, new { Sid = sId });
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<InvoiceViewModel>> GetVoucherListAsync(int year, int month)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var parameters = new
                {
                    Year = year,
                    MonthNo = month

                };

                var vouchers = await _db.QueryAsync<InvoiceViewModel>(
                    "USP_GET_JOURNAL_VOUCHER",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return vouchers.AsList();
            }
        }

        public async Task<int> GetMaxJournalId()
        {
            var jId = 0;


            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {

                    jId = await _db.QueryFirstOrDefaultAsync<int>("SELECT MAX(Jid) FROM Journal", commandTimeout:30);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return jId;

            
        }

        public async Task<int> SaveJournalsAsync(List<JouralView> journals)
        {
            int result = 0;
            try
            {
                
                using (var connection = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var jId = await connection.QueryFirstOrDefaultAsync<int>("SELECT MAX(Jid) + 1 FROM Journal", commandTimeout: 30);

                    foreach (JouralView journal in journals)
                    {
                        journal.Description = !string.IsNullOrEmpty(journal.Description) ? journal.Description.Replace("'", "`") : "";

                        string sql = @"
                        INSERT INTO journal (jid, sid, description, debt, credit, Jdate, PostDate, UserID)
                        VALUES (@JId, @SId, @Description, @Debt, @Credit, @JDate, @PostDate, @UserId);";

                        var parameters = new
                        {
                            jId,
                            journal.sid,
                            Description = journal.Description,
                            journal.Debt,
                            journal.Credit,
                            journal.jDate,
                            journal.PostDate,
                            journal.UserID
                        };


                        // Execute query asynchronously
                        await connection.ExecuteAsync(sql, parameters);
                        result = jId;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;

        }

        public async Task<string> MakeJournalVoucherAsync(int jId, string postDate)
        {
            var result = "";
            try
            {
                using (var connection = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new { JId = jId, PostDate = postDate };
                    string sql = "USP_MakeJournalVoucher @JId, @PostDate";

                    await connection.ExecuteAsync(sql, parameters);
                    result = "Success";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return result;

        }

    }
}