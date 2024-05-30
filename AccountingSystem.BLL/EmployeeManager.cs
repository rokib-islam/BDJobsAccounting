using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Abstractions.Repository;
using AccountingSystem.Models.AccountViewModels;

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
    }
}
