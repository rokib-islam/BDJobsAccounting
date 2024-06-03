using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class EmployeeModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeIdNo { get; set; }
        public string EmployeeName { get; set; }
        public string FathersName { get; set; }
        public string MothersName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string BloodGroup { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string NID { get; set; }
        public string ETin { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string Designation { get; set; }
        public string EmploymentStatus { get; set; }
        public string Rank { get; set; }
        public bool SupervisoryRole { get; set; }
        public int SupervisorId { get; set; }
        public string SupervisorName { get; set; }
        public string JobLocation { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public bool PDFActivation { get; set; }
        public DateTime PDFActivationStartDate { get; set; }
        public DateTime PDFActivationEndDate { get; set; }
        public bool ActiveStatus { get; set; }
        public DateTime ResignationDate { get; set; }
        public string BankAcName { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string BankAcNo { get; set; }
        public string RoutingNo { get; set; }
        public string BranchCode { get; set; }
        public string BkashNo { get; set; }
        public int DefaultPaymentMode { get; set; }
        public string Department { get; set; }
        public string Function { get; set; }
        public string IncrementEffectiveMonth { get; set; }
        public bool OTApplicable { get; set; }
        public bool LunchAllowance { get; set; }
        public string HusbandOrWifeName { get; set; }
        public string AcknowledgementNo { get; set; }
        public string ReturnYear { get; set; }
    }
}
