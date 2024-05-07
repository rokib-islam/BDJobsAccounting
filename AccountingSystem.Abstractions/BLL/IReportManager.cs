using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.Abstractions.BLL
{
    public interface IReportManager
    {
        Task<List<InvoiceReport>> GetInvoiceReportAsync(string invoiceNo);
        Task<List<ChalanReport>> GetChalanReportNew(string invoiceNo);
        Task<List<TrialBalanceRptModel>> GetTrialBalanceReportAsync(string type, string startingDate, string endDate);
        Task<List<LabelReport>> GetLabelReport(string type, string list);
        Task<List<LoadVatTaxCollectionDataModel_Response>> LoadVatTaxCollectionData(LoadVatTaxCollectionDataModel_Request model);
    }
}
