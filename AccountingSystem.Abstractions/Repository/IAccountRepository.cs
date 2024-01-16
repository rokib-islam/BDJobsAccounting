using AccountingSystem.Models.AccountDbModels;

namespace AccountingSystem.Abstractions.Repository
{
    public interface IAccountRepository
    {
        Task<Users> GetUsers(string userName, string password);
        Task<List<Users>> GetSpecificUser();

    }
}
