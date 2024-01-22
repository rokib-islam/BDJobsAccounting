using AccountingSystem.Abstractions.Repository;
using AccountingSystem.AppLicationDbContext.AccountingDatabase;
using AccountingSystem.Models.AccountViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Security.Cryptography;

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
        public async Task<List<JobListV2ViewModel>> GetOnlineJobList(string CName, int Verified, int LedgerID)
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
                    var joblist = await _db.QueryAsync<JobListV2ViewModel>("USP_ONLINE_JOB_LIST_V2", parameters, commandType: CommandType.StoredProcedure);

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
            bool isUploaded = true;

            try
            {
                string query = "SELECT id FROM tmpJobs WHERE Invoice_no != '' AND Submitted = 0";

                using (var connection = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                   var cId = await connection.QueryFirstOrDefaultAsync<int>(query);
                    if (cId > 0)
                    { 
                        isUploaded = false; 
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
            int row = 0;

            try
            {
                var cmdtext = "usp_Acc_Download_Jobs";

                using (var connection = new SqlConnection(_DBCon.GetConnectionString("OnlineConnection")))
                {
                    var parameters = new
                    {
                        FromDate = fromDate,
                        ToDate = toDate,
                        IsPNPL = PNPL
                    };

                    var data = await connection.QueryAsync<CorpJobViewModel>(cmdtext, parameters, commandType: CommandType.StoredProcedure);

                    if (data.Any())
                    {
                        var dataTable = CreateDataTable();
                        PopulateDataTable(dataTable, data);

                        using (var secondConnection = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                        {
                            await secondConnection.ExecuteAsync("USP_Download_Online_Jobs",
                                new { DownloadOnlineJob = dataTable.AsTableValuedParameter("dbo.DownloadOnlineJob") },
                                commandType: CommandType.StoredProcedure);
                        }


                        row = data.Count();
                    }
                }
            }
            catch (Exception ex)
            {

                row = -1;
            }

            return row;
        }

        private DataTable CreateDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("CP_ID", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Acc_Id", typeof(int));
            dataTable.Columns.Add("JP_ID", typeof(int));
            dataTable.Columns.Add("JobTitle", typeof(string));
            dataTable.Columns.Add("P", typeof(DateTime));
            dataTable.Columns.Add("DeadLine", typeof(DateTime));
            dataTable.Columns.Add("BillingContact", typeof(string));
            dataTable.Columns.Add("Designation", typeof(string));
            dataTable.Columns.Add("Count_JP_ID", typeof(int));
            dataTable.Columns.Add("OPID", typeof(int));
            dataTable.Columns.Add("AddType", typeof(int));
            dataTable.Columns.Add("RegionalJob", typeof(int));
            dataTable.Columns.Add("BlueCollar", typeof(int));
            dataTable.Columns.Add("VerifiedCompany", typeof(int));
            // Add other columns as needed
            return dataTable;
        }

        private void PopulateDataTable(DataTable dataTable, IEnumerable<CorpJobViewModel> data)
        {
            foreach (var item in data)
            {
                dataTable.Rows.Add(
                    item.CP_ID,
                    item.Name,
                    item.Acc_ID,
                    item.JP_ID,
                    item.JobTitle,
                    item.P,
                    item.Deadline,
                    item.BillingContact,
                    item.Designation,
                    item.Count_JP_ID,
                    item.OPID,
                    item.AdType,
                    item.RegionalJob,
                    item.BlueCollar,
                    item.VerifiedCompany
                );
            }
        }
    }
}
