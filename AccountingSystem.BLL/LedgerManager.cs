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

    }
}
