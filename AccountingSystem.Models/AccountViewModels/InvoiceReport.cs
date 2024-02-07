using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class InvoiceReport
    {
        public string Invoice_No { get; set; }
        public DateTime InvSendDt { get; set; }
        public string CName { get; set; }
        public string sbname { get; set; }
        public double amount { get; set; }
        public string comments { get; set; }
        public string bname { get; set; }
        public string designation { get; set; }
        public string RefNo { get; set; }
        public string VATRegNo { get; set; }
        public string AccountTitle { get; set; }
        public string BankACNo { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string BankRoutingNo { get; set; }
        public string SwiftCode { get; set; }
        public string Email { get; set; }
        public string CompanyId { get; set; }
        public string AccPersonMail { get; set; }
        public string SignatureImage { get; set; }
    }
}
