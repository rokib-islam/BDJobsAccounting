using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.Abstractions.BLL
{
    public interface IPaymentManager
    {
        Task<IEnumerable<CashCollectionViewModel>> GetCashCollectionAsync(string id);
        Task<string> InsertCashCollectionAsync(InsertCashCollectionViewModel cashCollection);
        Task<string> UpdateCashCollection(InsertCashCollectionViewModel cashCollection);
        Task<string> UnpaidCashCollectionAsync(UnpaidCashCollection model);

        Task<List<BankInformationModel>> GetBankInformation();
        Task<List<LoadPfPaymentDataResponseModel>> LoadPfPaymentData(LoadPfPaymentDataModel model);
    }
}
