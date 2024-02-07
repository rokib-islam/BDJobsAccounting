using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.Abstractions.BLL
{
    public interface IReportManager
    {
        Task<List<InvoiceReport>> GetInvoiceReportAsync(string invoiceNo);
    }
}
