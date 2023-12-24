using AccountingSystem.Abstractions.Repository.Base;
using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.Abstractions.Repository
{
    public interface IJournalRepository //: IRepository<Journal>
    {
        Task<List<Journal>> GetJournalListAsync(DateTime fromDate, DateTime toDate);

        Task<List<JouralView>> VireJournal(DateTime fromDate, DateTime toDate);
    }
}