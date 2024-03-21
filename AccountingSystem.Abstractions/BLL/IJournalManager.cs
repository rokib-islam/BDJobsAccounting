using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.Abstractions.BLL
{
    public interface IJournalManager //: IManager<Journal>
    {
        Task<List<JouralView>> GetJournalListAsync(int pageNo, int pageSize, int isPreview, string dateType, string startDate, string endDate, int ledgerId, string ledgerName, int companyId, int approvedBy, int postedBy, int isApproved);

        Task<string> GetClosingDateAsync();
        Task<string> UpdateSalesJournalAsync(UpdateSalesJournal updateInfo);
        Task<Journal> GetJournalBySIdAsync(int sId);
        Task<List<Invoice>> GetVoucherListAsync(int year, int month);

        Task<object> GetJournalsForTrialBalance(string pageNo, string pageSize, string tno, string fromDate, string endDate);
    }
}