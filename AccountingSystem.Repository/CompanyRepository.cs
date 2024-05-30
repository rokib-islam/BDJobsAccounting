using AccountingSystem.Abstractions.Repository;
using AccountingSystem.AppLicationDbContext.AccountingDatabase;
using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Models.AccountViewModels;
using AccountingSystem.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace AccountingSystem.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AccountingDbContext _context;
        private readonly IConfiguration _DBCon;

        public CompanyRepository(AccountingDbContext context, IConfiguration config) //: base(context)
        {
            _context = context;
            _DBCon = config;
        }
        public async Task<List<DistrictList>> GetDistricts()
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var result = await _db.QueryAsync<DistrictList>("Select DistrictID, DistrictName From DistrictList Order By DistrictName", new { });
                return result.ToList();
            }
        }
        public async Task<List<CompanyViewModel>> GetOnlineCompanyList(int radio)
        {
            var radioParam = "";

            if (radio == 0)
            {
                radioParam = "New";

            }
            else
            {
                radioParam = "All";
            }
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ViewType", radioParam);

                // Execute the stored procedure using Dapper
                var result = await _db.QueryAsync<CompanyViewModel>("[dbo].[USP_ONLINE_COMPANY_LIST]", parameters, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }

        }
        public async Task<List<CompanyViewModel>> GetCompanyListByKey(string Key)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var query = "SELECT TOP 30 Id,Name + ' (' + CONVERT(varchar, Cp_id) + ')' Name, BlackListed, AnyVatExemption FROM Company WHERE Name LIKE @Key";
                var parameters = new { Key = "%" + Key + "%" };

                var result = await _db.QueryAsync<CompanyViewModel>(query, parameters);
                return result.ToList();
            }

        }

        public async Task<List<CompanyViewModel>> GetOnlineCompanyInfo(int cpId)
        {
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("OnlineConnection")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@ComId", cpId);

                    // Execute the stored procedure using Dapper
                    var result = await _db.QueryAsync<CompanyViewModel>("[dbo].[USP_Acc_Download_ComInfo]", parameters, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public async Task<List<CompanyViewModel>> GetCompanyById(int cpId)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@CompanyId", cpId);

                // Execute the stored procedure using Dapper
                var result = await _db.QueryAsync<CompanyViewModel>("[dbo].[GetCompanyDetailsById]", parameters, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }

        }
        public async Task<List<CompanyViewModel>> CheckCompany(string name)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {

                var query = "SELECT Id, Name, Address, City, Phone, Email, Fax, Contact_Person, Designation, Balance, BlackListed, CP_ID, AccContactName, VATRegNo, VATRegAdd, DistrictID, UpazilaID, BankID FROM Company WHERE Name = @Name";

                var parameters = new DynamicParameters();
                parameters.Add("@Name", name);

                var result = await _db.QueryAsync<CompanyViewModel>(query, parameters);
                return result.ToList();
            }
        }
        public async Task<int> InsertUpdateOnlineCompany(CompanyInsertUpdateViewModel FromData)
        {
            int msg = 0;
            try
            {
                var parameters = new
                {
                    Action = FromData.Action,
                    CP_ID = FromData.CpId,
                    CompanyName = FromData.Name,
                    Address = FromData.Address,
                    City = FromData.City,
                    Phone = FromData.Phone,
                    Email = FromData.Email,
                    Contact_Person = FromData.CPerson,
                    Designation = "te",//FromData.Designation,
                    CompanyID = FromData.CompanyId,
                    DistrictID = FromData.DistrictId,
                    Type = FromData.Type
                };

                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    await _db.QueryAsync<CompanyViewModel>("USP_INSERT_UPDATE_ONLINE_COMPANY", parameters, commandType: CommandType.StoredProcedure);
                    msg = 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
                msg = 0;
            }
            return msg;
        }

        public async Task UpdateProfile(CompanyInsertUpdateViewModel FromData)
        {
            int cId = 0;

            if (FromData.Action == "Insert")
            {
                try
                {
                    using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                    {
                        cId = await _db.QueryFirstOrDefaultAsync<int>("SELECT id FROM company WHERE name=@Name AND cp_id=@CpId;", FromData);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                cId = Convert.ToInt32(FromData.CompanyId);
            }

            var query = "UPDATE dbo_Company_Profiles SET acc_Id=@cId WHERE cp_id=@CpId;";

            using (var _db = new SqlConnection(_DBCon.GetConnectionString("OnlineConnection")))
            {
                try
                {
                    await _db.ExecuteAsync(query, new { cId, FromData.CpId });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<CompanyViewModel> CheckOnlineCompany(int id)
        {
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var result = await _db.QueryFirstOrDefaultAsync<CompanyViewModel>(@"
                SELECT DISTINCT C.Id, C.Name, C.Address, C.City, C.Phone, C.Email, C.Fax,
                CASE WHEN CP.Name IS NULL THEN C.Contact_Person ELSE CP.Name END As Contact_Person,
                CASE WHEN CP.Designation IS NULL THEN C.Designation ELSE CP.Designation END As Designation,
                C.Balance, C.BlackListed, C.CP_ID, C.AccContactName, C.VATRegNo, C.VATRegAdd, C.DistrictID, C.UpazilaID, C.BankID, C.VatChallanName, C.AccPersonMail, C.AccPersonContactNo
                FROM Company C
                LEFT JOIN ContactPersons CP ON C.Id = CP.CID AND CP.PType = 'Contact person'
                WHERE C.Id = @Id", new { Id = id });

                    return result;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public async Task<CompanyViewModel> SMSAlertGetOnlineCompanyInfoAsync(int cpId)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var parameters = new
                    {
                        CP_ID = cpId
                    };

                    var result = await _db.QueryFirstOrDefaultAsync<CompanyViewModel>("USP_SMSAlert_Get_OnLine_Company_Info", parameters,
                        commandType: CommandType.StoredProcedure);

                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<IEnumerable<CompanyViewModel>> SMSAlertGetOnlineCompanyListAsync(int radio)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var parameters = new
                    {
                        Type = radio == 0 ? "New" : ""
                    };

                    // Using Dapper's QueryAsync to get a list of results
                    var result = await _db.QueryAsync<CompanyViewModel>(
                        "USP_SMSAlert_Get_OnLine_Company_Info",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return result;
                }
                catch (Exception ex)
                {
                    // Handle exceptions or log them as needed
                    throw ex;
                }
            }
        }
        public async Task<object> GetContactPersonsOrJobTitle(string type, int? cId)
        {
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {

                    var parameters = new { Type = type, CompanyID = cId };

                    var result = await _db.QueryAsync("USP_ONLINE_SALE_INFO", parameters, commandType: CommandType.StoredProcedure);

                    if (type == "C")
                    {
                        var contactPersons = result.Select(r => new ContactPerson
                        {
                            Id = r.id,
                            Name = r.name,
                            Designation = r.Designation
                        }).ToList();

                        return contactPersons;
                    }
                    else
                    {
                        var jobList = result.Select(r => new JobViewModel
                        {
                            Jp_Id = r.jp_id,
                            CompanyName = r.BillingContact,
                            PostingDate = Convert.ToDateTime(r.postingDate).ToShortDateString(),
                            ValidDate = Convert.ToDateTime(r.ValidDate).ToShortDateString(),
                            Title = r.title,
                            Type = r.designation,
                            Email = r.Email,
                            Mobile = r.Mobile
                        }).ToList();

                        return jobList;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ContactPerson>> GetContactPersonsByCompanyId(int companyId)
        {
            var contactPersons = new List<ContactPerson>();

            try
            {
                string sqlQuery = "SELECT Id, CID, Name, Designation, PType, Email, Phone FROM ContactPersons WHERE CID = @CompanyId";

                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var result = await _db.QueryAsync<ContactPerson>(sqlQuery, new { CompanyId = companyId });

                    contactPersons.AddRange(result);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception as needed.
                throw ex;
            }

            return contactPersons;
        }
        public async Task<ContactPerson> GetContactPersonByIdAsync(int id)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {

                var result = await _db.QueryFirstOrDefaultAsync<ContactPerson>("SELECT Id, CID, Name, Designation, PType, Email, Phone FROM ContactPersons WHERE Id = @Id", new { Id = id });

                return result;
            }
        }
        public async Task InsertOrUpdateCPAsync(ContactPerson aContact, string actionType)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Action", actionType);
                parameters.Add("@Id", aContact.Id);
                parameters.Add("@CID", aContact.CId);
                parameters.Add("@Contact_Person", aContact.Name);
                parameters.Add("@Designation", aContact.Designation);
                parameters.Add("@Email", aContact.Email);
                parameters.Add("@Phone", aContact.Phone);
                parameters.Add("@PType", aContact.PType);

                await _db.ExecuteAsync("USP_IUD_ContactPerson", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task DeletePersonAsync(int id)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var sqlQuery = "DELETE FROM dbo.ContactPersons WHERE Id = @Id";

                await _db.ExecuteAsync(sqlQuery, new { Id = id });
            }
        }
        public async Task<CompanyViewModel> GetCompanyByNameAsync(string name, int id)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {

                var sqlQuery = "SELECT Id, Name, Address, City, Phone, Email, Fax, Contact_Person, Designation, Balance, BlackListed, CP_ID, AccContactName, VATRegNo, VATRegAdd, DistrictID, UpazilaID, BankID FROM Company WHERE Name = @Name AND Id <> @Id";

                var result = await _db.QueryFirstOrDefaultAsync<CompanyViewModel>(sqlQuery, new { Name = name, Id = id });

                return result;
            }
        }
        public async Task<string> InsertOrUpdateCompanyAsync(CompanyViewModel aCompany)
        {
            var res = "";
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Name", aCompany.Name);
                    parameters.Add("@Address", aCompany.Address);
                    parameters.Add("@City", aCompany.City);
                    parameters.Add("@Phone", aCompany.Phone);
                    parameters.Add("@Email", aCompany.Email);
                    parameters.Add("@Fax", aCompany.Fax);
                    parameters.Add("@Contact_Person", aCompany.ContactPerson);
                    parameters.Add("@Designation", aCompany.Designation);
                    parameters.Add("@BlackListed", aCompany.BlackListed);
                    parameters.Add("@CP_ID", aCompany.CP_Id);
                    parameters.Add("@AccContactName", aCompany.AccContactName);
                    parameters.Add("@VATRegNo", aCompany.VatRegNo);
                    parameters.Add("@VATRegAdd", aCompany.VatRegAdd);
                    parameters.Add("@Id", aCompany.Id);
                    parameters.Add("@DistrictID", aCompany.DistrictId);
                    parameters.Add("@BankID", aCompany.BankId);
                    parameters.Add("@VatChallanName", aCompany.VatChallanName);
                    parameters.Add("@AccPersonMail", aCompany.AccPersonMail);
                    parameters.Add("@AccPersonContactNo", aCompany.AccPersonContactNo);
                    parameters.Add("@AnyVatExemption", aCompany.AnyVatExemption);
                    parameters.Add("@VatExemptionReason", aCompany.VatExemptionReason);
                    parameters.Add("@CustomizeRate", aCompany.CustomizeRate);
                    parameters.Add("@AutoMail", aCompany.AutoMail);
                    parameters.Add("@Remarks", aCompany.Remarks);

                    await _db.ExecuteAsync(
                        "INSERT_UPDATE_COMPANY",
                        parameters,
                        commandType: CommandType.StoredProcedure);
                    res = "Success";
                }
            }
            catch (Exception ex)
            {
                res = ex.ToString();
            }
            return res;
        }
        public async Task DeleteCompanyAsync(int id)
        {
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    string sqlQuery = @"DELETE FROM dbo.Company WHERE Id = @Id;
                                        DELETE FROM dbo.ContactPersons WHERE CID = @Id;";

                    await _db.ExecuteAsync(sqlQuery, new { Id = id });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }


}


