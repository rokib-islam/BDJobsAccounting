using AccountingSystem.Models.AccountDbModels;

namespace AccountingSystem.Abstractions.Repository
{
    public interface IAccountRepository
    {
        Users GetUsers(string userName, string password);

    }
}
