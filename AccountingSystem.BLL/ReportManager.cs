using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Abstractions.Repository;
using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.BLL
{
    public class ReportManager : IReportManager
    {
        private readonly IReportRepository _repository;
        public ReportManager(IReportRepository repository) //: base(repository)
        {
            _repository = repository;
        }

        public Task<List<InvoiceReport>> GetInvoiceReportAsync(string invoiceNo)
        {
            return _repository.GetInvoiceReportAsync(invoiceNo);
        }
       
    }
}
