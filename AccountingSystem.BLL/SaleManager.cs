using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Abstractions.Repository;
using AccountingSystem.Models.AccountDbModels;
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
        public async Task<List<SalesPersonViewModel>> GetSalesPersons(int productID)
        {
            return await _repository.GetSalesPersons(productID);
        }

        public Task<string> SaveSale(SaveSalesDataViewModel data)
        {
            return _repository.SaveSale(data);
        }

        public Task<int[]> CheckOnlineJobsAsync(string tnolist, string cId)
        {
            return _repository.CheckOnlineJobsAsync(tnolist, cId);
        }

        public Task<IEnumerable<Sale>> GetSalesInfoAsync(string invoiceNo)
        {
            return _repository.GetSalesInfoAsync(invoiceNo);
        }

        public Task<int> DownloadSMSAlertAsync(int serviceId, string fDate, string tDate)
        {
            return _repository.DownloadSMSAlertAsync(serviceId, fDate, tDate);
        }

        public Task<List<PostingOPIDs>> PostSMSAlertApplyLimitSalePosting(PostSMSAlertApplyLimitSale Data)
        {
            return _repository.PostSMSAlertApplyLimitSalePosting(Data);

        }

        public Task<List<PostingOPIDs>> GetSMSAlertApplyLimitForOnlinePost(string OPIDs)
        {
            return _repository.GetSMSAlertApplyLimitForOnlinePost(OPIDs);
        }

        public Task<List<PostingOPIDs>> PostSMSAlertApplyLimitToOnline(string OPIDs, int CMorJS, int Type)
        {
            return _repository.PostSMSAlertApplyLimitToOnline(OPIDs, CMorJS, Type);
        }

        public Task<List<PostingOPIDs>> GetSMSAlertApplyLimit(GetSMSApplyLimit Data)
        {
            return _repository.GetSMSAlertApplyLimit(Data);
        }

        public Task<List<Ledger>> CheckJobTitle(int productId)
        {
            return _repository.CheckJobTitle(productId);
        }

        public Task<object> GetSales(string pageNo, string pageSize, int cId, int tno)
        {
            return _repository.GetSales(pageNo, pageSize, cId, tno);
        }

        public Task<IEnumerable<DeletedSalesViewModel>> GetDeletedSales(string pageNo, string pageSize, int cId)
        {
            return _repository.GetDeletedSales(pageNo, pageSize, cId);
        }

        public Task<string> UpdateSaleProduct(string oldSid, string tno, string newSid)
        {
            return _repository.UpdateSaleProduct(oldSid, tno, newSid);
        }

        public Task<string> DeleteSale(int tno, string deleteReason, int creditNote, DateTime deleteDate)
        {
            return _repository.DeleteSale(tno, deleteReason, creditNote, deleteDate);
        }
        public Task<string> MakeJournalOfSale(MakeJournalOfSales saleInfo)
        {
            return _repository.MakeJournalOfSale(saleInfo);
        }

        public Task<object> GetNumberOfId(string tno)
        {
            return _repository.GetNumberOfId(tno);
        }

        public Task<string> UpdateSaleInfoAsync(UpdateSalesInfo salesInfo)
        {
            return _repository.UpdateSaleInfoAsync(salesInfo);
        }

        public Task<string> UpdateSalePosted(MakeJournalOfSales saleInfo)
        {
            return _repository.UpdateSalePosted(saleInfo);
        }

        public Task<string> UpdateSaleContactPersonAndRefNo(string personId, string refNo, int salesPerson, string tno)
        {
            return _repository.UpdateSaleContactPersonAndRefNo(personId, refNo, salesPerson, tno);
        }
    }
}
