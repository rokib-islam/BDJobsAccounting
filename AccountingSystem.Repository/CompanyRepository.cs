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
        public async Task<List<Company>> GetOnlineCompanyList(int radio)
        {
            var radioParam = "";

            if (radio == 0)
            {
                radioParam = "All";

            }
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ViewType", radioParam);

                // Execute the stored procedure using Dapper
                var result = await _db.QueryAsync<Company>("[dbo].[USP_ONLINE_COMPANY_LIST]", parameters, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }

        }
        public async Task<List<Company>> GetCompanyListByKey(string Key)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var query = "SELECT TOP 10 Id, Name, BlackListed FROM Company WHERE Name LIKE @Key ORDER BY Name";
                var parameters = new { Key = "%" + Key + "%" };

                var result = await _db.QueryAsync<Company>(query, parameters);
                return result.ToList();
            }

        }

        public async Task<List<Company>> GetOnlineCompanyInfo(int cpId)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("OnlineConnection")))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ComId", cpId);

                // Execute the stored procedure using Dapper
                var result = await _db.QueryAsync<Company>("[dbo].[USP_Acc_Download_ComInfo]", parameters, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }

        }
        public async Task<List<Company>> GetCompanyById(int cpId)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@CompanyId", cpId);

                // Execute the stored procedure using Dapper
                var result = await _db.QueryAsync<Company>("[dbo].[GetCompanyDetailsById]", parameters, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }

        }
        public async Task<List<Company>> CheckCompany(string name)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {

                var query = "SELECT Id, Name, Address, City, Phone, Email, Fax, Contact_Person, Designation, Balance, BlackListed, CP_ID, AccContactName, VATRegNo, VATRegAdd, DistrictID, UpazilaID, BankID FROM Company WHERE Name = @Name";

                var parameters = new DynamicParameters();
                parameters.Add("@Name", name);

                var result = await _db.QueryAsync<Company>(query, parameters);
                return result.ToList();
            }
        }
        public async Task<List<Company>> InsertUpdateOnlineCompany(CompanyInsertUpdateViewModel FromData)
        {
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
                    Designation = FromData.Designation,
                    CompanyID = FromData.CompanyId,
                    DistrictID = FromData.DistrictId,
                    Type = FromData.Type
                };

                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var result = await _db.QueryAsync<Company>("USP_INSERT_UPDATE_ONLINE_COMPANY", parameters, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateProfile(CompanyInsertUpdateViewModel FromData)
        {
            int cId = 0;

            if (FromData.Action == "INSERT")
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

        public async Task<Company> CheckOnlineCompany(int id)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("OnlineConnection")))
            {
                var result = await _db.QueryFirstOrDefaultAsync<Company>(@"
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
        public async Task<Company> SMSAlertGetOnlineCompanyInfoAsync(int cpId)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var parameters = new
                    {
                        CP_ID = cpId
                    };

                    var result = await _db.QueryFirstOrDefaultAsync<Company>("USP_SMSAlert_Get_OnLine_Company_Info", parameters,
                        commandType: CommandType.StoredProcedure);

                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<IEnumerable<Company>> SMSAlertGetOnlineCompanyListAsync(int radio)
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
                    var result = await _db.QueryAsync<Company>(
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
                            JpId = r.jp_id,
                            CompanyName = r.BillingContact,
                            PostingDate = Convert.ToDateTime(r.postingDate).ToShortDateString(),
                            ValidDate = Convert.ToDateTime(r.ValidDate).ToShortDateString(),
                            Title = r.title,
                            Type = r.Designation
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



    }


}


