using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Abstractions.Repository;
using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.BLL
{
    public class PaymentManager : IPaymentManager
    {
        private readonly IPaymentRepository _repository;
        public PaymentManager(IPaymentRepository repository) //: base(repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<CashCollectionViewModel>> GetCashCollectionAsync(string id)
        {
            return _repository.GetCashCollectionAsync(id);
        }

        public Task<string> InsertCashCollectionAsync(InsertCashCollectionViewModel cashCollection)
        {
            return _repository.InsertCashCollectionAsync(cashCollection);
        }

        public Task<string> UnpaidCashCollectionAsync(UnpaidCashCollection model)
        {
            return _repository.UnpaidCashCollectionAsync(model);
        }

        public Task<string> UpdateCashCollection(InsertCashCollectionViewModel cashCollection)
        {
            return _repository.UpdateCashCollection(cashCollection);
        }

        public async Task<List<BankInformationModel>> GetBankInformation()
        {
            return await _repository.GetBankInformation();
        }

        public async Task<List<LoadPfPaymentDataResponseModel>> LoadPfPaymentData(LoadPfPaymentDataModel model)
        {
            return await _repository.LoadPfPaymentData(model);
        }
        public async Task<List<VatSectionModel>> GetVatSection()
        {
            return await _repository.GetVatSection();
        }

        public async Task<List<VatSectionModel>> GetVatRateAsync(int id)
        {
            return await _repository.GetVatRateAsync(id);
        }
        public async Task<string> InsertPaymentModule(PaymentModuleModel model)
        {
            return await _repository.InsertPaymentModule(model);
        }

    }
}
