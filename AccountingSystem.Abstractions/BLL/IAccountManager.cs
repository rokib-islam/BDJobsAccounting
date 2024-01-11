using AccountingSystem.Models.AccountDbModels;

namespace AccountingSystem.Abstractions.BLL
{
    public interface IAccountManager
    {
        Task<List<Users>> GetUsers(string userName, string password);
    }
}
