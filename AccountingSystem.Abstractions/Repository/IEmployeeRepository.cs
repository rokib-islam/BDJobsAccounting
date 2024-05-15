using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.Abstractions.Repository
{
    public interface IEmployeeRepository
    {
        // Users GetUsers(string userName, string password);
        Task<List<EmployeeModel>> GetEmployeeListByKey(string Key);
        Task<string> InsertProvidentFundPayment(InsertProvidentFundPaymentModel model);
    }
}
