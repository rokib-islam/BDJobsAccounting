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
        public Task<List<ChalanReport>> GetChalanReportNew(string invoiceNo)
        {
            return _repository.GetChalanReportNew(invoiceNo);
        }

        public Task<List<TrialBalanceRptModel>> GetTrialBalanceReportAsync(string type, string startingDate, string endDate)
        {
            return _repository.GetTrialBalanceReportAsync(type, startingDate, endDate);
        }

        public Task<List<LabelReport>> GetLabelReport(string type, string list)
        {
            return _repository.GetLabelReport(type, list);
        }

        public async Task<List<LoadVatTaxCollectionDataModel_Response>> LoadVatTaxCollectionData(LoadVatTaxCollectionDataModel_Request model)
        {
            return await _repository.LoadVatTaxCollectionData(model);
        }

        public async Task<List<JournalVoucherReport>> GetVoucherReportAsync(int Jid)
        {
            return await _repository.GetVoucherReportAsync(Jid);
        }
    }
}
