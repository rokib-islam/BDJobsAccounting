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

        public async Task<List<Journal>> GetJournalListAsync(DateTime fromDate, DateTime toDate)
        {
            return await _repository.GetJournalListAsync(fromDate, toDate);
        }

        public async Task<List<JouralView>> VireJournal(DateTime fromDate, DateTime toDate)
        {
            return await _repository.VireJournal(fromDate, toDate);
        }
    }
}