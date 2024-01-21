using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.Abstractions.BLL
{
    public interface ILedgerManager
    {
        //Users GetUsers(string userName, string password);

        Task<List<ServiceViewModel>> GetService(int sTypy);
        Task<List<LedgerListViewModel>> GetAllLedger(string isAdmin, string isAccount);
        Task<int> GetOnlineLedgerId(string onlineProduct);

    }
}
