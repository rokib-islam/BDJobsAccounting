using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Abstractions.Repository;

namespace AccountingSystem.BLL
{
    public class LedgerManager : ILedgerManager
    {
        private readonly ILedgerRepository _repository;
        public LedgerManager(ILedgerRepository repository) //: base(repository)
        {
            _repository = repository;
        }
        //public  Users GetUsers(string userName, string password)
        //{
        //    return  _repository.GetUsers(userName, password);
        //}
    }
}
