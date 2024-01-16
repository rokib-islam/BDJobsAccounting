using AccountingSystem.Models.AccountDbModels;

namespace AccountingSystem.Abstractions.Repository
{
    public interface IJournalRepository //: IRepository<Journal>
    {
        Task<List<Journal>> GetJournalListAsync(int pageNo, int pageSize, int isPreview, string dateType, string startDate, string endDate, int ledgerId, string ledgerName, int companyId, int approvedBy, int postedBy, int isApproved);


    }
}