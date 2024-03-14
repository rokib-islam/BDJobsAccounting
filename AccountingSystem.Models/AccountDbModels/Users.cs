using System.ComponentModel.DataAnnotations;

namespace AccountingSystem.Models.AccountDbModels
{

    public class Users
    {
        [Key]
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string UName { get; set; }
        public string PWord { get; set; }
        public string AccessRight { get; set; }
        public int CanApprove { get; set; }
        public string ApproveRight { get; set; }
        public int CanModifyAdmin { get; set; }
        public string AccessReports { get; set; }
        public int ValidUser { get; set; }
        public int AccountDep { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string SignatureImage { get; set; }
    }
}