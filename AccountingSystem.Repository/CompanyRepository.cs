using AccountingSystem.Abstractions.Repository;
using AccountingSystem.AppLicationDbContext.AccountingDatabase;
using AccountingSystem.Models.AccountDbModels;
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
    }

}
