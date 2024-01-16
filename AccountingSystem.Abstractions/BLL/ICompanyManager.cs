using AccountingSystem.Models.AccountDbModels;

namespace AccountingSystem.Abstractions.BLL
{
    public interface ICompanyManager
    {
        //Users GetUsers(string userName, string password);
        Task<List<DistrictList>> GetDistricts();
    }
}
