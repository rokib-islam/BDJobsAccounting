using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.Abstractions.BLL
{
    public interface IEmployeeManager
    {
        //Users GetUsers(string userName, string password);
        Task<List<EmployeeModel>> GetEmployeeListByKey(string key);
        Task<string> InsertProvidentFundPayment(InsertProvidentFundPaymentModel model);
    }
}
