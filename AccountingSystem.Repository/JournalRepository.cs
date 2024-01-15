using AccountingSystem.Abstractions.Repository;
using AccountingSystem.AppLicationDbContext.AccountingDatabase;
using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Models.AccountViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

        public async Task<List<Journal>> GetJournalListAsync(DateTime fromDate, DateTime toDate)
        {
            var parameter1 = new SqlParameter("@Parameter1", fromDate);
            var parameter2 = new SqlParameter("@Parameter2", toDate);

            //try
            //{
            //    var res = await _context.Database.SqlQuery<Journal>(@$"Select top 10 * from Journal").ToListAsync();

            //}
            //catch (Exception ed)
            //{ }
            //var res = await _context.Database.SqlQueryRaw<Journal>("Select top 10 * from Journal").ToListAsync();


            //     await context.Database.SqlQuery<PostSummary>(
            //    @$"exec GetRecentPostSummariesProc")
            //.ToListAsync();

            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                // Use QueryAsync instead of Query for asynchronous execution
                var result = await _db.QueryAsync<Journal>("select top 10 * from journal", new { });

                // Convert the result to a list
                return result.ToList();
            }


        }

        public async Task<List<JouralView>> VireJournal(DateTime fromDate, DateTime toDate)
        {

            var page = new SqlParameter("Page", 1);
            var PageSize = new SqlParameter("PageSize", 20);
            var preview = new SqlParameter("Preview", 20);
            var dateType = new SqlParameter("DateType", "PostDate");

            var sDate = new SqlParameter("StartDate", "10/01/2023");
            var eDate = new SqlParameter("EndDate", "10/01/2023");

            var res = await _context.Database
                .SqlQueryRaw<JouralView>($"EXEC [dbo].[USP_VIEW_JOURNAL_LIST] @Page, @PageSize,@Preview, @DateType, @StartDate, @EndDate", page, PageSize, preview, dateType, sDate, eDate)
                .ToListAsync();



            return res;
        }
    }
}