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
    }
}