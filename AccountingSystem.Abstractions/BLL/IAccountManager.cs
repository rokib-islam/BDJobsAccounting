using AccountingSystem.Models.AccountDbModels;

namespace AccountingSystem.Abstractions.BLL
{
    public interface IAccountManager
    {
        Task<Users> GetUsers(string userName, string password);

        Task<List<Users>> GetSpecificUser();
    }
}
