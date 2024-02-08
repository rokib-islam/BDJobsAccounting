using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.Abstractions.Repository
{
    public interface IReportRepository
    {
        Task<List<InvoiceReport>> GetInvoiceReportAsync(string invoiceNo);
        Task<List<ChalanReport>> GetChalanReportNew(string invoiceNo);

    }
}
