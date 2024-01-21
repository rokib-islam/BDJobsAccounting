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


    }
}
