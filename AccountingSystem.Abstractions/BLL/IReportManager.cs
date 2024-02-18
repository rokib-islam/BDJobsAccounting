using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.Abstractions.BLL
{
    public interface IReportManager
    {
        Task<List<InvoiceReport>> GetInvoiceReportAsync(string invoiceNo);
        Task<List<ChalanReport>> GetChalanReportNew(string invoiceNo);
        Task<List<Ledger>> GetTrialBalanceReportAsync(string type, string startingDate, string endDate);
    }
}
