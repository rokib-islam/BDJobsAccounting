using AccountingSystem.Abstractions.BLL;

namespace AccountingSystem.BLL
{
    public class EmployeeManager : IEmployeeManager
    {
        private readonly IEmployeeManager _repository;
        public EmployeeManager(IEmployeeManager repository) //: base(repository)
        {
            _repository = repository;
        }
        //public  Users GetUsers(string userName, string password)
        //{
        //    return  _repository.GetUsers(userName, password);
        //}
    }
}
