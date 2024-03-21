using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.Abstractions.Repository
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<CashCollectionViewModel>> GetCashCollectionAsync(string id);
        Task<string> InsertCashCollectionAsync(InsertCashCollectionViewModel cashCollection);
        Task<string> UpdateCashCollection(InsertCashCollectionViewModel cashCollection);
        Task<string> UnpaidCashCollectionAsync(UnpaidCashCollection model);
    }
}
