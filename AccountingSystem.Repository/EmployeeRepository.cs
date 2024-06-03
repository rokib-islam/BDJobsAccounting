using AccountingSystem.Abstractions.Repository;
using AccountingSystem.AppLicationDbContext.AccountingDatabase;
using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Models.AccountViewModels;
using Dapper;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<List<Department_Function_Rank_Model>> LoadAllDepartment()
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var result = await _db.QueryAsync<Department_Function_Rank_Model>("SELECT Id, DepartmentName FROM Department", new { });
                return result.ToList();
            }
        }

        public async Task<List<Department_Function_Rank_Model>> LoadAllFunction()
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var result = await _db.QueryAsync<Department_Function_Rank_Model>("SELECT Id, FunctionName FROM [Function]", new { });
                return result.ToList();
            }
        }

        public async Task<List<Department_Function_Rank_Model>> LoadAllRank()
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var result = await _db.QueryAsync<Department_Function_Rank_Model>("SELECT Id, RankName FROM Rank", new { });
                return result.ToList();
            }
        }

        public async Task<List<Department_Function_Rank_Model>> LoadSupervisor()
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var result = await _db.QueryAsync<Department_Function_Rank_Model>("SELECT EmployeeId, EmployeeName FROM EmployeeInfo WHERE Rank=4", new { });
                return result.ToList();
            }
        }

        public async Task<string> InsertOrUpdateEmployeeInfo(EmployeeModel model)
        {
            var res = "";
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@EmployeeId", model.EmployeeId);
                    parameters.Add("@EmployeeIdNo", model.EmployeeIdNo);
                    parameters.Add("@EmployeeName", model.EmployeeName);
                    parameters.Add("@FathersName", model.FathersName);
                    parameters.Add("@MothersName", model.MothersName);
                    parameters.Add("@HusbandOrWifeName", model.HusbandOrWifeName);
                    parameters.Add("@Gender", model.Gender);
                    parameters.Add("@DateOfBirth", model.DateOfBirth);
                    parameters.Add("@BloodGroup", model.BloodGroup);
                    parameters.Add("@MobileNo", model.MobileNo);
                    parameters.Add("@Email", model.Email);
                    parameters.Add("@NID", model.NID);
                    parameters.Add("@ETin", model.ETin);
                    parameters.Add("@PresentAddress", model.PresentAddress);
                    parameters.Add("@PermanentAddress", model.PermanentAddress);
                    parameters.Add("@Designation", model.Designation);
                    parameters.Add("@EmploymentStatus", model.EmploymentStatus);
                    parameters.Add("@Rank", model.Rank);
                    parameters.Add("@SupervisoryRole", model.SupervisoryRole);
                    parameters.Add("@SupervisorId", model.SupervisorId);
                    parameters.Add("@JobLocation", model.JobLocation);
                    parameters.Add("@IncrementEffectiveMonth", model.IncrementEffectiveMonth);
                    parameters.Add("@JoiningDate", model.JoiningDate);
                    parameters.Add("@EffectiveDate", model.EffectiveDate);
                    parameters.Add("@PDFActivation", model.PDFActivation);
                    parameters.Add("@PDFActivationStartDate", model.PDFActivationStartDate);
                    parameters.Add("@PDFActivationEndDate", model.PDFActivationEndDate);
                    parameters.Add("@ActiveStatus", model.ActiveStatus);
                    parameters.Add("@ResignationDate", model.ResignationDate);
                    parameters.Add("@OTApplicable", model.OTApplicable);
                    parameters.Add("@LunchAllowance", model.LunchAllowance);
                    parameters.Add("@BankAcName", model.BankAcName);
                    parameters.Add("@BankName", model.BankName);
                    parameters.Add("@BranchName", model.BranchName);
                    parameters.Add("@BankAcNo", model.BankAcNo);
                    parameters.Add("@RoutingNo", model.RoutingNo);
                    parameters.Add("@BranchCode", model.BranchCode);
                    parameters.Add("@BkashNo", model.BkashNo);
                    parameters.Add("@DefaultPaymentMode", model.DefaultPaymentMode);
                    parameters.Add("@Department", model.Department);
                    parameters.Add("@Function", model.Function);


                    await _db.ExecuteAsync(
                        "InsertOrUpdateEmployeeInfo",
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

        public async Task<List<EmployeeModel>> LoadEmployeeInfoById(int id)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                var result = await _db.QueryAsync<EmployeeModel>("USP_LoadEmployeeInfoById", parameters, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }

        }

        public async Task<string> InsertOrUpdateAcknowledgement([FromBody] Acknowledgement_GrossSalary_TA_Model model)
        {
            var res = "";
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@EmployeeId", model.EmployeeId);
                    parameters.Add("@AcknowledgementNo", model.AcknowledgementNo);
                    parameters.Add("@ReturnYear", model.ReturnYear);
                    parameters.Add("@ID", model.ID);

                    await _db.ExecuteAsync("InsertOrUpdateTaxReturnAcknowledgement", parameters,commandType: CommandType.StoredProcedure);
                    res = "Success";
                }
            }
            catch (Exception ex)
            {
                res = ex.ToString();
            }
            return res;
        }

        public async Task<List<Acknowledgement_GrossSalary_TA_Model>> GetAcknowledgementNoByEmployeeId(int employeeId)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var query = "SELECT tr.ID, tr.AcknowledgementNo, tr.ReturnYear FROM TaxReturnAcknowledgement tr LEFT JOIN EmployeeInfo e ON tr.EmployeeId = e.EmployeeId WHERE e.EmployeeId = @EmployeeId";
                var parameters = new { EmployeeId = employeeId };

                var result = await _db.QueryAsync<Acknowledgement_GrossSalary_TA_Model>(query, parameters);
                return result.ToList();
            }

        }

    }
}
