using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Abstractions.Repository;
using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.BLL
{
    public class LedgerManager : ILedgerManager
    {
        private readonly ILedgerRepository _repository;
        public LedgerManager(ILedgerRepository repository) //: base(repository)
        {
            _repository = repository;
        }
        public async Task<List<ServiceViewModel>> GetService(int sTypy)
        {
            return await _repository.GetService(sTypy);
        }
        public async Task<List<LedgerListViewModel>> GetAllLedger(string isAdmin, string isAccount)
        {
            return await _repository.GetAllLedger(isAdmin, isAccount);
        }
        public async Task<int> GetOnlineLedgerId(string onlineProduct)
        {
            return await _repository.GetOnlineLedgerId(onlineProduct);
        }
        public async Task<List<Ledger>> GetProducts(int admin, int account, string groupname, string isAll, string isI, int isVatType)
        {
            return await _repository.GetProducts(admin, account, groupname, isAll, isI, isVatType);
        }

        public Task<List<Ledger>> GetLedgersWithBalance()
        {
            return _repository.GetLedgersWithBalance();
        }

        public Task<List<Ledger>> GetAllLedgers()
        {
            return _repository.GetAllLedgers();
        }

        public Task SaveLedgerAsync(Ledger aLedger)
        {
            return _repository.SaveLedgerAsync(aLedger);
        }

        public Task<int> UpdateLedgerAsync(Ledger aLedger)
        {
            return _repository.UpdateLedgerAsync(aLedger);
        }

        public Task<string> DeleteLedgerAsync(int ledgerId)
        {
            return _repository.DeleteLedgerAsync(ledgerId);
        }
    }
}
