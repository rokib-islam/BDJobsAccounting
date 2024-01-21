using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.Abstractions.BLL
{
    public interface ISaleManager
    {
        //Users GetUsers(string userName, string password);
        Task FixDownloadIssue();
        Task<List<OnlineJobViewModel>> GetOnlineJobList(string CName, int Verified, int LedgerID);
        Task<List<JobViewModel>> GetJobs(int cpId, string date, int adType, int adRegion);
        Task DeleteOnlineJob(int jpId);
        Task<bool> IsAllUploaded();
        Task<int> DownloadJobs(string fromDate, string toDate, int PNPL);

    }
}
