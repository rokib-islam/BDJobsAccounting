using AccountingSystem.Models.AccountDbModels;

namespace AccountingSystem.Abstractions.BLL
{
    public interface ICompanyManager
    {
        //Users GetUsers(string userName, string password);
        Task<List<DistrictList>> GetDistricts();
        Task<List<Company>> GetOnlineCompanyList(int radio);
        Task<List<Company>> GetCompanyListByKey(string startingKey);
    }
}
