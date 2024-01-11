using AccountingSystem.Models.AccountDbModels;

namespace AccountingSystem.Abstractions.Repository
{
    public interface IAccountRepository
    {
        Task<List<Users>> GetUsers(string userName, string password);
    }
}
