﻿using AccountingSystem.Models.AccountViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.Abstractions.BLL
{
    public interface IEmployeeManager
    {
        //Users GetUsers(string userName, string password);
        Task<List<EmployeeModel>> GetEmployeeListByKey(string key);
        Task<List<VendorModel>> GetVendorListByKey(string key);
        Task<string> InsertProvidentFundPayment(InsertProvidentFundPaymentModel model);
        Task<List<Department_Function_Rank_Model>> LoadAllDepartment();
        Task<List<Department_Function_Rank_Model>> LoadAllFunction();
        Task<List<Department_Function_Rank_Model>> LoadAllRank();
        Task<List<Department_Function_Rank_Model>> LoadSupervisor();
        Task<string> InsertOrUpdateEmployeeInfo(EmployeeModel model);
        Task<List<EmployeeModel>> LoadEmployeeInfoById(int id);
        Task<string> InsertOrUpdateAcknowledgement([FromBody] Acknowledgement_GrossSalary_TA_Model model);
        Task<List<Acknowledgement_GrossSalary_TA_Model>> GetAcknowledgementNoByEmployeeId(int employeeId);
        Task<string> InsertOrUpdateGrossSalary([FromBody] Acknowledgement_GrossSalary_TA_Model model);
        Task<List<Acknowledgement_GrossSalary_TA_Model>> GetGrossSalaryByEmployeeId(int employeeId);
        Task<string> InsertOrUpdateTA([FromBody] Acknowledgement_GrossSalary_TA_Model model);
        Task<List<Acknowledgement_GrossSalary_TA_Model>> GetTaByEmployeeId(int employeeId);
        Task<List<EmployeeModel>> LoadAllEmployeeInfo();
        Task<string> ImportACS();
    }
}
