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
    }
}
