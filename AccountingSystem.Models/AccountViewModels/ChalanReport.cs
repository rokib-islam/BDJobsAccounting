using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class ChalanReport
    {
        public string ChallanNo { get; set; }
        public string ServiceType { get; set; }
        public int quantity { get; set; }
        public double oneItemPrice { get; set; }
        public double TotalPrice { get; set; }
        public double TotalVat { get; set; }
        public string Contact_Person { get; set; }
        public string VATRegNo { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string adress { get; set; }
        public double priceWithVat { get; set; }
        public string UserName { get; set; }
        public string UserDegignation { get; set; }
        public string ServiceName { get; set; }
        public double VatRate { get; set; }
        public string Rtype { get; set; }
        public string CreditNote { get; set; }
        public string DeleteReason { get; set; }
        public int IsCancel { get; set; }
        public int ViewCnt { get; set; }
        public string Condition { get; set; }
        public string ViewStatus { get; set; }
        public string InvoiceNo { get; set; }
        public string fDate { get; set; }
        public string tDate { get; set; }
        public int CompanyId { get; set; }
        public string AccPersonMail { get; set; }
        public string SignatureImage { get; set; }
    }
}
