using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Abstractions.Repository;
using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.BLL
{
    public class JournalManager : IJournalManager
    {
        private readonly IJournalRepository _repository;

        public JournalManager(IJournalRepository repository) //: base(repository)
        {
            _repository = repository;
        }

        public async Task<List<JouralView>> GetJournalListAsync(int pageNo, int pageSize, int isPreview, string dateType, string startDate, string endDate, int ledgerId, string ledgerName, int companyId, int approvedBy, int postedBy, int isApproved)
        {
            return await _repository.GetJournalListAsync(pageNo, pageSize, isPreview, dateType, startDate, endDate, ledgerId, ledgerName, companyId, approvedBy, postedBy, isApproved);
        }
        public async Task<string> GetClosingDateAsync()
        {
            return await _repository.GetClosingDateAsync();
        }
        public Task<string> UpdateSalesJournalAsync(UpdateSalesJournal updateInfo)
        {
            return _repository.UpdateSalesJournalAsync(updateInfo);
        }

        public Task<Journal> GetJournalBySIdAsync(int sId)
        {
            return _repository.GetJournalBySIdAsync(sId);
        }

        public Task<List<Invoice>> GetVoucherListAsync(int year, int month)
        {
            return _repository.GetVoucherListAsync(year, month);
        }

        public async Task<object> GetJournalsForTrialBalance(string pageNo, string pageSize, string tno, string fromDate, string endDate)
        {
            return await _repository.GetJournalsForTrialBalance(pageNo, pageSize, tno, fromDate, endDate);
        }
    }
}