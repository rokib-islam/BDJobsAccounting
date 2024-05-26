using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class LoadPfPaymentDataResponseModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string DepartmentName { get; set; }
        public string PDFActivationStartDate { get; set; }
        public string PDFActivationEndDate { get; set; }
        public decimal PFDeduction { get; set; }
        public decimal EmpOpeningBal { get; set; }
        public decimal ComOpeningBal { get; set; }
        public decimal EmpRangeContribution { get; set; }
        public decimal ComRangeContribution { get; set; }
        public decimal TotalEmpContribution { get; set; }
        public decimal TotalComContribution { get; set; }
        public decimal TotalContribution { get; set; }
        public decimal ForfeitureAmount { get; set; }
        public decimal PaidEmpContribution { get; set; }
        public decimal PaidComContribution { get; set; }
        public decimal ClosingBalanceBeforeProfit { get; set; }
        public decimal EmpInvestmentIncome { get; set; }
        public decimal ComInvestmentIncome { get; set; }
        public decimal ClosingEmpContribution { get; set; }
        public decimal ClosingComContribution { get; set; }
    }
}
