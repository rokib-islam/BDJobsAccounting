using AccountingSystem.Abstractions.Repository;
using AccountingSystem.AppLicationDbContext.AccountingDatabase;
using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;
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
                // Use parameterized query to avoid SQL injection
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

                //Update Company section to online 

                if (FromData.Action == "INSERT")
                {

                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
