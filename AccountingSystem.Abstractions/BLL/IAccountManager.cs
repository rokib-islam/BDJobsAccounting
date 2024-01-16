using AccountingSystem.Models.AccountDbModels;

namespace AccountingSystem.Abstractions.BLL
{
    public interface IAccountManager
    {
        Users GetUsers(string userName, string password);

        List<Users> GetSpecificUser();
    }
}
