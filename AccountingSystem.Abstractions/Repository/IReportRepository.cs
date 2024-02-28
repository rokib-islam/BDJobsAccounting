using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.Abstractions.Repository
{
    public interface IReportRepository
    {
        Task<List<InvoiceReport>> GetInvoiceReportAsync(string invoiceNo);
        Task<List<ChalanReport>> GetChalanReportNew(string invoiceNo);
        Task<List<TrialBalanceRptModel>> GetTrialBalanceReportAsync(string type, string startingDate, string endDate);

    }
}
