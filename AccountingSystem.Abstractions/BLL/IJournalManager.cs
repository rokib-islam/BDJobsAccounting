using AccountingSystem.Abstractions.BLL.Base;
using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.Abstractions.BLL
{
    public interface IJournalManager //: IManager<Journal>
    {
        Task<List<Journal>> GetJournalListAsync(DateTime fromDate, DateTime toDate);

        Task<List<JouralView>> VireJournal(DateTime fromDate, DateTime toDate);
    }
}