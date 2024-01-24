using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.Abstractions.Repository
{
    public interface ISaleRepository
    {
        // Users GetUsers(string userName, string password);
        Task FixDownloadIssue();
        Task<List<JobListV2ViewModel>> GetOnlineJobList(string CName, int Verified, int LedgerID);
        Task<List<JobViewModel>> GetJobs(int cpId, string date, int adType, int adRegion);
        Task DeleteOnlineJob(int jpId);
        Task<bool> IsAllUploaded();
        Task<int> DownloadJobs(string fromDate, string toDate, int PNPL);
        Task<List<SalesPersonViewModel>> GetSalesPersonsAsync(int productID);
        Task<string> SaveSale(SaveSalesDataViewModel data);
        Task<int[]> CheckOnlineJobsAsync(string tnolist, string cId);
        Task<IEnumerable<Sale>> GetSalesInfoAsync(string invoiceNo);
        Task<int> DownloadSMSAlertAsync(int serviceId, string fDate, string tDate);
        Task<List<PostingOPIDs>> PostSMSAlertApplyLimitSalePosting(PostSMSAlertApplyLimitSale Data);
        Task<List<PostingOPIDs>> GetSMSAlertApplyLimitForOnlinePost(string OPIDs);
        Task<List<PostingOPIDs>> PostSMSAlertApplyLimitToOnline(string OPIDs, int CMorJS, int Type);
        Task<List<PostingOPIDs>> GetSMSAlertApplyLimit(GetSMSApplyLimit Data);

    }
}
