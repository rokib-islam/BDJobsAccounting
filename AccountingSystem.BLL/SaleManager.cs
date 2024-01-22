using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Abstractions.Repository;
using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.BLL
{
    public class SaleManager : ISaleManager
    {
        private readonly ISaleRepository _repository;
        public SaleManager(ISaleRepository repository) //: base(repository)
        {
            _repository = repository;
        }

        public async Task FixDownloadIssue()
        {
            await _repository.FixDownloadIssue();
        }
        public async Task<List<JobListV2ViewModel>> GetOnlineJobList(string CName, int Verified, int LedgerID)
        {
            return await _repository.GetOnlineJobList(CName, Verified, LedgerID);
        }
        public async Task<List<JobViewModel>> GetJobs(int cpId, string date, int adType, int adRegion)
        {
            return await _repository.GetJobs(cpId, date, adType, adRegion);
        }
        public async Task DeleteOnlineJob(int jpId)
        {
            await _repository.DeleteOnlineJob(jpId);
        }
        public async Task<bool> IsAllUploaded()
        {
            return await _repository.IsAllUploaded();
        }
        public async Task<int> DownloadJobs(string fromDate, string toDate, int PNPL)
        {
            return await _repository.DownloadJobs(fromDate, toDate, PNPL);
        }





    }
}
