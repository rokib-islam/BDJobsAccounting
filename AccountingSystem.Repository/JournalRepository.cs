using AccountingSystem.Abstractions.Repository;
using AccountingSystem.AppLicationDbContext.AccountingDatabase;
using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Models.AccountViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

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


    }
}