using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Abstractions.Repository;
using AccountingSystem.Models.AccountDbModels;
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

        public async Task<List<LedgerListViewModel>> GetAllEveryLedger(string isCashCollection)
        {
            return await _repository.GetAllEveryLedger(isCashCollection);
        }
        public async Task<int> GetOnlineLedgerId(string onlineProduct)
        {
            return await _repository.GetOnlineLedgerId(onlineProduct);
        }
        public async Task<List<LedgerViewModel>> GetProducts(int admin, int account, string groupname, string isAll, string isI, int isVatType)
        {
            return await _repository.GetProducts(admin, account, groupname, isAll, isI, isVatType);
        }

        public Task<List<LedgerViewModel>> GetLedgersWithBalance()
        {
            return _repository.GetLedgersWithBalance();
        }

        public Task<List<LedgerViewModel>> GetAllLedgers()
        {
            return _repository.GetAllLedgers();
        }

        public Task SaveLedgerAsync(LedgerViewModel aLedger)
        {
            return _repository.SaveLedgerAsync(aLedger);
        }

        public Task<int> UpdateLedgerAsync(LedgerViewModel aLedger)
        {
            return _repository.UpdateLedgerAsync(aLedger);
        }

        public Task<string> DeleteLedgerAsync(int ledgerId)
        {
            return _repository.DeleteLedgerAsync(ledgerId);
        }

        //public async Task<List<LedgerViewModel>> GetProductListByKey(string Key)
        //{
        //    return await _repository.GetProductListByKey(Key);
        //}

        public async Task<List<LedgerViewModel>> GetProductList()
        {
            return await _repository.GetProductList();
        }

        public async Task<List<LedgerViewModel>> GetProductById(int pId)
        {
            return await _repository.GetProductById(pId);
        }

        public async Task<List<LoadServiceListModel>> LoadServiceList(LoadServiceListModel model)
        {
            return await _repository.LoadServiceList(model);
        }
    }
}
