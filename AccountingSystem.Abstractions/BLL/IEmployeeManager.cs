using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.Abstractions.BLL
{
    public interface IEmployeeManager
    {
        //Users GetUsers(string userName, string password);
        Task<List<EmployeeModel>> GetEmployeeListByKey(string key);
        Task<string> InsertProvidentFundPayment(InsertProvidentFundPaymentModel model);
        Task<List<Department_Function_Rank_Model>> LoadAllDepartment();
        Task<List<Department_Function_Rank_Model>> LoadAllFunction();
        Task<List<Department_Function_Rank_Model>> LoadAllRank();
        Task<List<Department_Function_Rank_Model>> LoadSupervisor();
    }
}
