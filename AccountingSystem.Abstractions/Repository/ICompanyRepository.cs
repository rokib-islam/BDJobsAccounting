using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Models.AccountViewModels;
using AccountingSystem.Web.Models;

namespace AccountingSystem.Abstractions.Repository
{
    public interface ICompanyRepository
    {
        Task<List<DistrictList>> GetDistricts();
        Task<List<CompanyViewModel>> GetOnlineCompanyList(int radio);
        Task<List<CompanyViewModel>> GetCompanyListByKey(string startingKey);
        Task<List<CompanyViewModel>> GetOnlineCompanyInfo(int cpId);
        Task<List<CompanyViewModel>> GetCompanyById(int cpId);
        Task<List<CompanyViewModel>> CheckCompany(string name);
        Task<int> InsertUpdateOnlineCompany(CompanyInsertUpdateViewModel FromData);
        Task UpdateProfile(CompanyInsertUpdateViewModel FromData);
        Task<CompanyViewModel> CheckOnlineCompany(int id);
        Task<CompanyViewModel> SMSAlertGetOnlineCompanyInfoAsync(int cpId);
        Task<IEnumerable<CompanyViewModel>> SMSAlertGetOnlineCompanyListAsync(int radio);
        Task<object> GetContactPersonsOrJobTitle(string type, int? cId);
        Task<List<ContactPerson>> GetContactPersonsByCompanyId(int companyId);
        Task<ContactPerson> GetContactPersonByIdAsync(int id);
        Task InsertOrUpdateCPAsync(ContactPerson aContact, string actionType);
        Task DeletePersonAsync(int id);
        Task<CompanyViewModel> GetCompanyByNameAsync(string name, int id);
        Task<string> InsertOrUpdateCompanyAsync(CompanyViewModel aCompany);
        Task DeleteCompanyAsync(int id);

    }
}
