﻿using AccountingSystem.Abstractions.Repository;
using AccountingSystem.AppLicationDbContext.AccountingDatabase;
using AccountingSystem.Models.AccountDbModels;
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
                throw ex;
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

                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var cId = await _db.QueryFirstOrDefaultAsync<int>(query);
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

        #region download online jobs
        public async Task<int> DownloadJobs(string fromDate, string toDate, int PNPL)
        {
            int row = 0;

            try
            {
                var cmdtext = "usp_Acc_Download_Jobs";

                using (var OnlineConn = new SqlConnection(_DBCon.GetConnectionString("OnlineConnection")))
                {
                    var parameters = new
                    {
                        FromDate = fromDate,
                        ToDate = toDate,
                        IsPNPL = PNPL
                    };

                    var data = await OnlineConn.QueryAsync<CorpJobViewModel>(cmdtext, parameters, commandType: CommandType.StoredProcedure);

                    if (data.Any())
                    {
                        var dataTable = CreateDataTable();
                        PopulateDataTable(dataTable, data);

                        using (var Connection = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                        {
                            await Connection.ExecuteAsync("USP_Download_Online_Jobs",
                                new { DownloadOnlineJob = dataTable.AsTableValuedParameter("dbo.DownloadOnlineJob") },
                                commandType: CommandType.StoredProcedure);
                        }


                        row = data.Count();
                    }
                }
            }
            catch (Exception)
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
        #endregion download online jobs

        public async Task<List<SalesPersonViewModel>> GetSalesPersons(int productID)
        {
            var salesPersons = new List<SalesPersonViewModel>();

            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new { DepartmentID = productID };

                    salesPersons = (await _db.QueryAsync<SalesPersonViewModel>("USP_GET_Sales_Person", parameters, commandType: CommandType.StoredProcedure)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return salesPersons;
        }
        public async Task<string> SaveSale(SaveSalesDataViewModel data)
        {
            var result = "Success";
            try
            {

                var parameters = new
                {
                    UserID = data.UserId,
                    CID = data.CId,
                    PCODE = data.PCode,
                    SDate = data.FromDate,
                    EDate = data.ToDate,
                    JDate = data.JournalDate,
                    SalesPrice = data.SalesPrice,
                    BillingPerson = data.BillingPerson.Replace("'", "`"),
                    Designation = data.Designation.Replace("'", "`"),
                    Comments = data.Comment.Replace("'", "`"),
                    Duration = data.Duration,
                    NumberOfInvoices = data.NoOfInvoice,
                    RefNo = data.RefNo,
                    TaxId = data.TypeId,
                    Tax = data.Vat,
                    JP_ID = data.JpId,
                    Title = data.JobTitle.Replace("'", "`"),
                    WorkshopDate = data.WorkshopDate,
                    SalesPerson = data.SPerson
                };

                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    await _db.ExecuteAsync("USP_INSERT_SALE", parameters, commandType: CommandType.StoredProcedure);
                }


            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        public async Task<int[]> CheckOnlineJobsAsync(string tnolist, string cId)
        {
            var data = new int[2];
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var result = await _db.QueryFirstOrDefaultAsync<dynamic>(
                        "USP_CHECK_ONLINE_JOBS",
                        new { C_ID = cId, TNO = tnolist },
                        commandType: CommandType.StoredProcedure
                    );

                    if (result != null)
                    {
                        data[0] = Convert.ToInt32(result.OnlineJobs);
                        data[1] = Convert.ToInt32(result.TotalJobs);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return data;
        }
        public async Task<IEnumerable<Sale>> GetSalesInfoAsync(string invoiceNo)
        {
            var sales = new List<Sale>();

            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {

                    var query = @"SELECT s.TNO, l.SBName, i1.Amount, i1.Id, i1.comments
                          FROM sales AS s, ledger AS l, InvoiceSceduler AS i1, InvoiceList AS i
                          WHERE i.Id = i1.invoice_id AND i1.TNO = s.tno AND s.PCode = l.id AND i.Invoice_No = @InvoiceNo";

                    var result = await _db.QueryAsync<Sale>(query, new { InvoiceNo = invoiceNo });

                    sales.AddRange(result);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return sales;
        }

        #region download sms alert
        public async Task<int> DownloadSMSAlertAsync(int serviceId, string fDate, string tDate)
        {
            int row = 0;

            try
            {
                var parameters = new
                {
                    ServiceID = serviceId,
                    FromDate = fDate,
                    ToDate = tDate
                };

                using (var _Onlinedb = new SqlConnection(_DBCon.GetConnectionString("OnlineConnection")))
                {
                    var result = await _Onlinedb.QueryAsync<SMSAlertApplyLimit>("Accounting.USP_ACC_SMSAlert_ApplyLimit_Download_New", parameters, commandType: CommandType.StoredProcedure);

                    if (result.Any())
                    {
                        var dataTable = CreateSMSAlertDataTable();
                        PopulateSMSAlertDataTable(dataTable, result);

                        using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                        {
                            await _db.ExecuteAsync("USP_SMSAlert_ApplyLimit_Download",
                                new { SMSAlertApplyLimt = dataTable.AsTableValuedParameter("dbo.SMSAlertApplyLimt") },
                                commandType: CommandType.StoredProcedure);
                        }

                        row = result.Count();
                    }
                }
            }
            catch (Exception)
            {
                row = -1;
            }

            return row;
        }

        private DataTable CreateSMSAlertDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("ServiceGroup", typeof(int));
            dataTable.Columns.Add("OPID", typeof(int));
            dataTable.Columns.Add("ServiceName", typeof(string));
            dataTable.Columns.Add("P_ID", typeof(int));
            dataTable.Columns.Add("Quantity", typeof(int));
            dataTable.Columns.Add("PaidAmount", typeof(decimal));
            dataTable.Columns.Add("TransID", typeof(string));
            dataTable.Columns.Add("TransDate", typeof(string)); // Assuming string for simplicity, use DateTime if applicable
            dataTable.Columns.Add("PaidBy", typeof(string));
            dataTable.Columns.Add("ServiceID", typeof(int));
            dataTable.Columns.Add("Amount", typeof(decimal));
            dataTable.Columns.Add("DetailInfo", typeof(string));

            return dataTable;
        }

        private void PopulateSMSAlertDataTable(DataTable dataTable, IEnumerable<SMSAlertApplyLimit> data)
        {
            foreach (var item in data)
            {
                dataTable.Rows.Add(
                    item.ServiceGroup,
                    item.OPID,
                    item.ServiceName,
                    item.P_ID,
                    item.Quantity,
                    item.PaidAmount,
                    item.TransID,
                    item.TransDate,
                    item.PaidBy,
                    item.ServiceID,
                    item.Amount,
                    item.DetailInfo
                );
            }
        }
        #endregion  download sms alert

        public async Task<List<PostingOPIDs>> PostSMSAlertApplyLimitSalePosting(PostSMSAlertApplyLimitSale Data)
        {
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new
                    {
                        OPIDs = Data.OPIDs,
                        UserID = Data.UserID,
                        CPID = Data.ComID,
                        ServiceGroup = Data.ServiceGroup,
                        ServiceID = Data.ServiceID,
                        ReceivedDate = Data.JournalDate
                    };

                    var result = (await _db.QueryAsync<PostingOPIDs>("USP_SMSAlert_ApplyLimit_Sale_Postings", parameters,
                        commandType: CommandType.StoredProcedure)).ToList();

                    return result;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<List<PostingOPIDs>> GetSMSAlertApplyLimitForOnlinePost(string OPIDs)
        {
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new
                    {
                        UpdateSubmit = OPIDs
                    };

                    var result = await _db.QueryAsync<PostingOPIDs>("USP_SMSAlert_ApplyLimit_PostToOnline", parameters,
                        commandType: CommandType.StoredProcedure);

                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<List<PostingOPIDs>> PostSMSAlertApplyLimitToOnline(string OPIDs, int CMorJS, int Type)
        {
            using (var _Onlinedb = new SqlConnection(_DBCon.GetConnectionString("OnlineConnection")))
            {
                var parameters = new
                {
                    OPIDs = OPIDs,
                    CMorJS = CMorJS,
                    Type = Type
                };

                var result = await _Onlinedb.QueryAsync<PostingOPIDs>("USP_ACC_SMSAlert_ApplyLimit_PostStatus", parameters,
                    commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }
        public async Task<List<PostingOPIDs>> GetSMSAlertApplyLimit(GetSMSApplyLimit Data)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var parameters = new
                {
                    ServiceGroup = Data.ServiceGroup,
                    ServiceID = Data.ServiceID,
                    PaymentMethod = Data.PaymentMethod,
                    FromDate = Data.FromDate,
                    ToDate = Data.ToDate,
                    PageNo = Data.PageNo,
                    PageSize = Data.PageSize
                };

                var result = await _db.QueryAsync<PostingOPIDs>("USP_SMSAlert_ApplyLimit", parameters,
                    commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }

        public async Task<List<Ledger>> CheckJobTitle(int productId)
        {
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new { ProductID = productId };

                    var result = await _db.QueryAsync<Ledger>("USP_CheckJobTitle", parameters, commandType: CommandType.StoredProcedure);

                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
