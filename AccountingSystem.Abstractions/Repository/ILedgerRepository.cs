using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.Abstractions.Repository
{
    public interface ILedgerRepository
    {
        // Users GetUsers(string userName, string password);
        Task<List<ServiceViewModel>> GetService(int sTypy);

    }
}
