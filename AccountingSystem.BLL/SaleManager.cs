using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Abstractions.Repository;

namespace AccountingSystem.BLL
{
    public class SaleManager : ISaleManager
    {
        private readonly ISaleRepository _repository;
        public SaleManager(ISaleRepository repository) //: base(repository)
        {
            _repository = repository;
        }
        //public  Users GetUsers(string userName, string password)
        //{
        //    return  _repository.GetUsers(userName, password);
        //}
    }
}
