using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.Abstractions.BLL
{
    public interface ILedgerManager
    {
        //Users GetUsers(string userName, string password);

        Task<List<ServiceViewModel>> GetService(int sTypy);
        Task<List<LedgerListViewModel>> GetAllLedger(string isAdmin, string isAccount);
        Task<int> GetOnlineLedgerId(string onlineProduct);
        Task<List<Ledger>> GetProducts(int admin, int account, string groupname, string isAll, string isI, int isVatType);
        Task<List<Ledger>> GetLedgersWithBalance();
        Task<List<Ledger>> GetAllLedgers();
        Task SaveLedgerAsync(Ledger aLedger);
        Task<int> UpdateLedgerAsync(Ledger aLedger);
        Task<string> DeleteLedgerAsync(int ledgerId);
    }
}
