using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Abstractions.Repository;
using AccountingSystem.Models.AccountDbModels;

namespace AccountingSystem.BLL
{
    public class AccountManager : IAccountManager
    {
        private readonly IAccountRepository _repository;
        public AccountManager(IAccountRepository repository) //: base(repository)
        {
            _repository = repository;
        }
        public Users GetUsers(string userName, string password)
        {
            return _repository.GetUsers(userName, password);
        }
    }
}
