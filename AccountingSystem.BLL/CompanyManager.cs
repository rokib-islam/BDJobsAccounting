using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Abstractions.Repository;
using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Web.Models;

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
        public async Task<List<Company>> GetOnlineCompanyInfo(int cpId)
        {
            return await _repository.GetOnlineCompanyInfo(cpId);
        }
        public async Task<List<Company>> GetCompanyById(int cpId)
        {
            return await _repository.GetCompanyById(cpId);
        }
        public async Task<List<Company>> CheckCompany(string name)
        {
            return await _repository.CheckCompany(name);
        }
        public async Task<List<Company>> InsertUpdateOnlineCompany(CompanyInsertUpdateViewModel FromData)
        {
            return await _repository.InsertUpdateOnlineCompany(FromData);
        }
        public async Task UpdateProfile(CompanyInsertUpdateViewModel FromData)
        {
            await _repository.UpdateProfile(FromData);
        }
        public async Task<Company> CheckOnlineCompany(int id)
        {
            return await _repository.CheckOnlineCompany(id);
        }


    }
}
