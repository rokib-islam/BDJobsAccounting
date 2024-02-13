using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Web.Models;

namespace AccountingSystem.Abstractions.Repository
{
    public interface ICompanyRepository
    {
        Task<List<DistrictList>> GetDistricts();
        Task<List<Company>> GetOnlineCompanyList(int radio);
        Task<List<Company>> GetCompanyListByKey(string startingKey);
        Task<List<Company>> GetOnlineCompanyInfo(int cpId);
        Task<List<Company>> GetCompanyById(int cpId);
        Task<List<Company>> CheckCompany(string name);
        Task<int> InsertUpdateOnlineCompany(CompanyInsertUpdateViewModel FromData);
        Task UpdateProfile(CompanyInsertUpdateViewModel FromData);
        Task<Company> CheckOnlineCompany(int id);
        Task<Company> SMSAlertGetOnlineCompanyInfoAsync(int cpId);
        Task<IEnumerable<Company>> SMSAlertGetOnlineCompanyListAsync(int radio);
        Task<object> GetContactPersonsOrJobTitle(string type, int? cId);
        Task<List<ContactPerson>> GetContactPersonsByCompanyId(int companyId);
        Task<ContactPerson> GetContactPersonByIdAsync(int id);
        Task InsertOrUpdateCPAsync(ContactPerson aContact, string actionType);
        Task DeletePersonAsync(int id);
        Task<Company> GetCompanyByNameAsync(string name, int id);
        Task<string> InsertOrUpdateCompanyAsync(Company aCompany);
        Task DeleteCompanyAsync(int id);

    }
}
