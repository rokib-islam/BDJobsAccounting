using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.Abstractions.Repository
{
    public interface IReportRepository
    {
        Task<List<InvoiceReport>> GetInvoiceReportAsync(string invoiceNo);

    }
}
