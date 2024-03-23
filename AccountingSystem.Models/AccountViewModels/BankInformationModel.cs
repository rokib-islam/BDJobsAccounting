using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class BankInformationModel
    {
        public int BankID { get; set; }
        public string AccountTitle { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string BankACNo { get; set; }
        public string BankRoutingNo { get; set; }
        public string SwiftCode { get; set; }
        public string Email { get; set; }
    }
}
