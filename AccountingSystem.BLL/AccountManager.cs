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
        public async Task<Users> GetUsers(string userName, string password)
        {
            return await _repository.GetUsers(userName, password);
        }

        public async Task<List<Users>> GetSpecificUser()
        {
            return await _repository.GetSpecificUser();
        }

        public async Task<List<Users>> GetApprovers()
        {
            return await _repository.GetApprovers();
        }
    }
}
