using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.Abstractions.Repository
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<CashCollection>> GetCashCollectionAsync(string id);
        Task<string> InsertCashCollectionAsync(InsertCashCollectionViewModel cashCollection);
        Task<string> UpdateCashCollection(InsertCashCollectionViewModel cashCollection);
        Task<string> UnpaidCashCollectionAsync(UnpaidCashCollection model);
    }
}
