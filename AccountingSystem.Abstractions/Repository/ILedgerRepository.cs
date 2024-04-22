using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.Abstractions.Repository
{
    public interface ILedgerRepository
    {
        // Users GetUsers(string userName, string password);
        Task<List<ServiceViewModel>> GetService(int sTypy);
        Task<List<LedgerListViewModel>> GetAllLedger(string isAdmin, string isAccount);
        Task<List<LedgerListViewModel>> GetAllEveryLedger(string isCashCollection);
        Task<int> GetOnlineLedgerId(string onlineProduct);
        Task<List<LedgerViewModel>> GetProducts(int admin, int account, string groupname, string isAll, string isI, int isVatType);
        Task<List<LedgerViewModel>> GetLedgersWithBalance();
        Task<List<LedgerViewModel>> GetAllLedgers();
        Task SaveLedgerAsync(LedgerViewModel aLedger);
        Task<int> UpdateLedgerAsync(LedgerViewModel aLedger);
        Task<string> DeleteLedgerAsync(int ledgerId);
        Task<List<LedgerViewModel>> GetProductListByKey(string Key);
        Task<List<LedgerViewModel>> GetProductById(int pId);
    }
}
