using AccountingSystem.Models.AccountViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.Abstractions.Repository
{
    public interface IEmployeeRepository
    {
        // Users GetUsers(string userName, string password);
        Task<List<EmployeeModel>> GetEmployeeListByKey(string Key);
        Task<string> InsertProvidentFundPayment(InsertProvidentFundPaymentModel model);
        Task<List<Department_Function_Rank_Model>> LoadAllDepartment();
        Task<List<Department_Function_Rank_Model>> LoadAllFunction();
        Task<List<Department_Function_Rank_Model>> LoadAllRank();
        Task<List<Department_Function_Rank_Model>> LoadSupervisor();
        Task<string> InsertOrUpdateEmployeeInfo(EmployeeModel model);
        Task<List<EmployeeModel>> LoadEmployeeInfoById(int id);
        Task<string> InsertOrUpdateAcknowledgement([FromBody] Acknowledgement_GrossSalary_TA_Model model);
        Task<List<Acknowledgement_GrossSalary_TA_Model>> GetAcknowledgementNoByEmployeeId(int employeeId);
    }
}
