using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Abstractions.Repository;
using AccountingSystem.Models.AccountViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.BLL
{
    public class EmployeeManager : IEmployeeManager
    {
        private readonly IEmployeeRepository _repository;
        public EmployeeManager(IEmployeeRepository repository) //: base(repository)
        {
            _repository = repository;
        }
        //public  Users GetUsers(string userName, string password)
        //{
        //    return  _repository.GetUsers(userName, password);
        //}

        public async Task<List<EmployeeModel>> GetEmployeeListByKey(string key)
        {
            return await _repository.GetEmployeeListByKey(key);
        }

        public async Task<List<VendorModel>> GetVendorListByKey(string key)
        {
            return await _repository.GetVendorListByKey(key);
        }
        public async Task<string> InsertProvidentFundPayment(InsertProvidentFundPaymentModel model)
        {
            return await _repository.InsertProvidentFundPayment(model);
        }

        public async Task<List<Department_Function_Rank_Model>> LoadAllDepartment()
        {
            return await _repository.LoadAllDepartment();
        }

        public async Task<List<Department_Function_Rank_Model>> LoadAllFunction()
        {
            return await _repository.LoadAllFunction();
        }

        public async Task<List<Department_Function_Rank_Model>> LoadAllRank()
        {
            return await _repository.LoadAllRank();
        }

        public async Task<List<Department_Function_Rank_Model>> LoadSupervisor()
        {
            return await _repository.LoadSupervisor();
        }

        public async Task<string> InsertOrUpdateEmployeeInfo(EmployeeModel model)
        {
            return await _repository.InsertOrUpdateEmployeeInfo(model);
        }

        public async Task<List<EmployeeModel>> LoadEmployeeInfoById(int id)
        {
            return await _repository.LoadEmployeeInfoById(id);
        }

        public async Task<string> InsertOrUpdateAcknowledgement([FromBody] Acknowledgement_GrossSalary_TA_Model model)
        {
            return await _repository.InsertOrUpdateAcknowledgement(model);
        }

        public async Task<List<Acknowledgement_GrossSalary_TA_Model>> GetAcknowledgementNoByEmployeeId(int employeeId)
        {
            return await _repository.GetAcknowledgementNoByEmployeeId(employeeId);
        }

        public async Task<string> InsertOrUpdateGrossSalary([FromBody] Acknowledgement_GrossSalary_TA_Model model)
        {
            return await _repository.InsertOrUpdateGrossSalary(model);
        }
        
        public async Task<List<Acknowledgement_GrossSalary_TA_Model>> GetGrossSalaryByEmployeeId(int employeeId)
        {
            return await _repository.GetGrossSalaryByEmployeeId(employeeId);
        }
        
        public async Task<string> InsertOrUpdateTA([FromBody] Acknowledgement_GrossSalary_TA_Model model)
        {
            return await _repository.InsertOrUpdateTA(model);
        }
        public async Task<List<Acknowledgement_GrossSalary_TA_Model>> GetTaByEmployeeId(int employeeId)
        {
            return await _repository.GetTaByEmployeeId(employeeId);
        }
        public async Task<List<EmployeeModel>> LoadAllEmployeeInfo()
        {
            return await _repository.LoadAllEmployeeInfo();
        }
        public async Task<string> ImportACS()
        {
            return await _repository.ImportACS();
        }
    }
}
