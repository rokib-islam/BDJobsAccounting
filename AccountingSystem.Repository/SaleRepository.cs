using AccountingSystem.Abstractions.Repository;
using AccountingSystem.AppLicationDbContext.AccountingDatabase;
using AccountingSystem.Models.AccountViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace AccountingSystem.Repository
{
    public class SaleRepository : ISaleRepository
    {
        private readonly AccountingDbContext _context;
        private readonly IConfiguration _DBCon;

        public SaleRepository(AccountingDbContext context, IConfiguration config) //: base(context)
        {
            _context = context;
            _DBCon = config;
        }

        public async Task FixDownloadIssue()
        {
            var query = "UPDATE tmpJobs SET Submitted = 1 WHERE Invoice_No IS NOT NULL AND Submitted = 0";

            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                await _db.ExecuteAsync(query);
            }
        }
        public async Task<List<OnlineJobViewModel>> GetOnlineJobList(string CName, int Verified, int LedgerID)
        {
            try
            {
                var parameters = new
                {
                    CName,
                    Verified,
                    LedgerID
                };

                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var joblist = await _db.QueryAsync<OnlineJobViewModel>("USP_ONLINE_JOB_LIST_V2", parameters, commandType: CommandType.StoredProcedure);

                    return joblist.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<JobViewModel>> GetJobs(int cpId, string date, int adType, int adRegion)
        {
            var parameters = new
            {
                CP_ID = cpId,
                PostingDate = date,
                AddType = adType,
                Region = adRegion
            };

            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var result = await _db.QueryAsync<JobViewModel>("USP_GetJobTitles_List", parameters, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task DeleteOnlineJob(int jpId)
        {
            string query = "DELETE FROM tmpJobs WHERE jp_id = @JpId";

            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                await _db.ExecuteAsync(query, new { JpId = jpId });
            }
        }

        public async Task<bool> IsAllUploaded()
        {
            bool isUploaded = false;

            try
            {
                string query = "SELECT id FROM tmpJobs WHERE Invoice_no != '' AND Submitted = 0";

                using (var connection = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        isUploaded = await reader.ReadAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }

            return isUploaded;
        }

        public async Task<int> DownloadJobs(string fromDate, string toDate, int PNPL)
        {
            return 0;
        }
    }
}
