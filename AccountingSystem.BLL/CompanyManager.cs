using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Abstractions.Repository;
using AccountingSystem.Models.AccountDbModels;

namespace AccountingSystem.BLL
{
    public class CompanyManager : ICompanyManager
    {
        private readonly ICompanyRepository _repository;
        public CompanyManager(ICompanyRepository repository) //: base(repository)
        {
            _repository = repository;
        }

        public async Task<List<DistrictList>> GetDistricts()
        {
            return await _repository.GetDistricts();
        }
        public async Task<List<Company>> GetOnlineCompanyList(int radio)
        {
            return await _repository.GetOnlineCompanyList(radio);
        }
        public async Task<List<Company>> GetCompanyListByKey(string key)
        {
            return await _repository.GetCompanyListByKey(key);
        }

    }
}
