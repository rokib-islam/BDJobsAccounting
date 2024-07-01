using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.Abstractions.Repository
{
    public interface ISaleRepository
    {
        // Users GetUsers(string userName, string password);
        Task FixDownloadIssue();
        Task<List<JobListV2ViewModel>> GetOnlineJobList(string FromDate, string ToDate, string CName, int Verified, int LedgerID);
        Task<List<JobViewModel>> GetJobs(int cpId, string date, int adType, int adRegion);
        Task DeleteOnlineJob(int jpId);
        Task<bool> IsAllUploaded();
        Task<int> DownloadJobs(string fromDate, string toDate, int PNPL);
        Task<List<SalesPersonViewModel>> GetSalesPersons(int productID);
        Task<string> SaveSale(SaveSalesDataViewModel data);
        Task<int[]> CheckOnlineJobsAsync(string tnolist, string cId);
        Task<IEnumerable<SaleViewModel>> GetSalesInfoAsync(string invoiceNo);
        Task<int> DownloadSMSAlertAsync(int serviceId, string fDate, string tDate);
        Task<List<PostingOPIDs>> PostSMSAlertApplyLimitSalePosting(PostSMSAlertApplyLimitSale Data);
        Task<List<PostingOPIDs>> GetSMSAlertApplyLimitForOnlinePost(string OPIDs);
        Task<List<PostingOPIDs>> PostSMSAlertApplyLimitToOnline(string OPIDs, int CMorJS, int Type);
        Task<List<PostingOPIDs>> GetSMSAlertApplyLimit(GetSMSApplyLimit Data);
        Task<List<LedgerViewModel>> CheckJobTitle(int productId);
        Task<object> GetSales(string pageNo, string pageSize, int cId, int tno);
        Task<IEnumerable<DeletedSalesViewModel>> GetDeletedSales(string pageNo, string pageSize, int cId);
        Task<string> UpdateSaleProduct(string oldSid, string tno, string newSid);
        Task<string> DeleteSale(int tno, string deleteReason, int creditNote, DateTime deleteDate);
        Task<string> MakeJournalOfSale(MakeJournalOfSales saleInfo);
        Task<object> GetNumberOfId(string tno);
        Task<string> UpdateSaleInfoAsync(UpdateSalesInfo salesInfo);

        Task<string> UpdateSalePosted(MakeJournalOfSales saleInfo);
        Task<string> UpdateSaleContactPersonAndRefNo(string personId, string refNo, int salesPerson, string tno);
        Task<List<SalesPerson>> GetSalesPersonListByKey(string startingKey);
        Task<int> DownloadCandidateMonetizationAsync();
        Task<MonetizationPosting> PostSMSAlertApplyLimitSalePostingNew(string ServiceName);
        Task<MonetizationPosting> PostSMSAlertApplyLimitSalePostingNew_Manual(string ServiceName, string ReceivedDate, string FromDate, string ToDate);
        Task<List<AutoBillingModel_Response>> AutoBillingData(AutoBillingModel model);
        Task<int> SMSAlertApplyLimitCountForBilling(string ServiceName);
        Task<dynamic> PostSMSAlertApplyLimitSalePostingEmployeer(CVAndSMSPurchesModel data);
        Task<List<VatAndTaxModel_Response>> GetCompanyVatAndTax(VatAndTaxModel_Request model);
        Task<List<SalesReconciliationModel_Response>> GetSalesReconciliation(SalesReconciliationModel_Request model);
    }
}
