using AccountingSystem.Models.AccountDbModels;

namespace AccountingSystem.Abstractions.Repository
{
    public interface ICompanyRepository
    {
        Task<List<DistrictList>> GetDistricts();
        Task<List<Company>> GetOnlineCompanyList(int radio);

    }
}
