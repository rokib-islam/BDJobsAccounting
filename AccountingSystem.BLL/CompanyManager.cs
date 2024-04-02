using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Abstractions.Repository;
using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Models.AccountViewModels;
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
        public async Task<List<CompanyViewModel>> GetOnlineCompanyList(int radio)
        {
            return await _repository.GetOnlineCompanyList(radio);
        }
        public async Task<List<CompanyViewModel>> GetCompanyListByKey(string key)
        {
            return await _repository.GetCompanyListByKey(key);
        }
        public async Task<List<CompanyViewModel>> GetOnlineCompanyInfo(int cpId)
        {
            return await _repository.GetOnlineCompanyInfo(cpId);
        }
        public async Task<List<CompanyViewModel>> GetCompanyById(int cpId)
        {
            return await _repository.GetCompanyById(cpId);
        }
        public async Task<List<CompanyViewModel>> CheckCompany(string name)
        {
            return await _repository.CheckCompany(name);
        }
        public async Task<int> InsertUpdateOnlineCompany(CompanyInsertUpdateViewModel FromData)
        {
            return await _repository.InsertUpdateOnlineCompany(FromData);
        }
        public async Task UpdateProfile(CompanyInsertUpdateViewModel FromData)
        {
            await _repository.UpdateProfile(FromData);
        }
        public async Task<CompanyViewModel> CheckOnlineCompany(int id)
        {
            return await _repository.CheckOnlineCompany(id);
        }

        public async Task<CompanyViewModel> SMSAlertGetOnlineCompanyInfoAsync(int cpId)
        {
            return await _repository.SMSAlertGetOnlineCompanyInfoAsync(cpId);
        }

        public async Task<IEnumerable<CompanyViewModel>> SMSAlertGetOnlineCompanyListAsync(int radio)
        {
            return await _repository.SMSAlertGetOnlineCompanyListAsync(radio);
        }

        public Task<object> GetContactPersonsOrJobTitle(string type, int? cId)
        {
            return _repository.GetContactPersonsOrJobTitle(type, cId);
        }

        public Task<List<ContactPerson>> GetContactPersonsByCompanyId(int companyId)
        {
            return _repository.GetContactPersonsByCompanyId(companyId);
        }

        public Task<ContactPerson> GetContactPersonByIdAsync(int id)
        {
            return _repository.GetContactPersonByIdAsync(id);
        }

        public Task InsertOrUpdateCPAsync(ContactPerson aContact, string actionType)
        {
            return _repository.InsertOrUpdateCPAsync(aContact, actionType);
        }

        public Task DeletePersonAsync(int id)
        {
            return _repository.DeletePersonAsync(id);
        }

        public Task<CompanyViewModel> GetCompanyByNameAsync(string name, int id)
        {
            return _repository.GetCompanyByNameAsync(name, id);
        }

        public Task<string> InsertOrUpdateCompanyAsync(CompanyViewModel aCompany)
        {
            return _repository.InsertOrUpdateCompanyAsync(aCompany);
        }

        public Task DeleteCompanyAsync(int id)
        {
            return _repository.DeleteCompanyAsync(id);
        }
    }
}
