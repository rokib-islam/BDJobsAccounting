using AccountingSystem.Abstractions.Repository;
using AccountingSystem.AppLicationDbContext.AccountingDatabase;
using AccountingSystem.Models.AccountViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace AccountingSystem.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AccountingDbContext _context;
        private readonly IConfiguration _DBCon;

        public EmployeeRepository(AccountingDbContext context, IConfiguration config) //: base(context)
        {
            _context = context;
            _DBCon = config;
        }
        //public Users GetUsers(string userName, string password)
        //{
        //    using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
        //    {
        //        // Use QueryFirstOrDefault instead of Query for getting a single result
        //        var result = _db.QueryFirstOrDefault<Users>("SELECT * FROM Users WHERE UName = @UName AND PWord = @PWord",
        //            new { UName = userName, PWord = password });

        //        return result;
        //    }
        //}

        public async Task<List<EmployeeModel>> GetEmployeeListByKey(string Key)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var query = "SELECT TOP 10 EmployeeId, EmployeeName FROM EmployeeInfo WHERE EmployeeName LIKE @Key ORDER BY EmployeeName";
                var parameters = new { Key = "%" + Key + "%" };

                var result = await _db.QueryAsync<EmployeeModel>(query, parameters);
                return result.ToList();
            }

        }

        public async Task<string> InsertProvidentFundPayment(InsertProvidentFundPaymentModel model)
        {
            var res = "";
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@PaymentDate", model.PaymentDate);
                    parameters.Add("@EmployeeId", model.EmployeeId);
                    parameters.Add("@LedgerId", model.LedgerId);
                    parameters.Add("@PaymentRef", model.PaymentRef);
                    parameters.Add("@EmpContribution_pay", model.EmpContribution_pay);
                    parameters.Add("@EmpContribution_adj", model.EmpContribution_adj);
                    parameters.Add("@ComContribution_pay", model.ComContribution_pay);
                    parameters.Add("@ComContribution_adj", model.ComContribution_adj);
                    parameters.Add("@EmpProfitCont_pay", model.EmpProfitCont_pay);
                    parameters.Add("@EmpProfitCont_adj", model.EmpProfitCont_adj);
                    parameters.Add("@ComProfitCont_pay", model.ComProfitCont_pay);
                    parameters.Add("@ComProfitCont_adj", model.ComProfitCont_adj);
                    parameters.Add("@UserId", model.UserId);
                    

                    await _db.ExecuteAsync(
                        "USP_InsertProvidentFundPayment",
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
    }
}
