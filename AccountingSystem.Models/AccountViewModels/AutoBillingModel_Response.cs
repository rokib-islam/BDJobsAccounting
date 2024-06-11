using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class AutoBillingModel_Response
    {
        public string ServiceName { get; set; }
        public string TransID { get; set; }
        public DateTime TransDate { get; set; }
        public string PaidBy { get; set; }
        public decimal Amount { get; set; }
        public string InvoiceNo { get; set; }
        public string TotalRecords { get; set; }
        public int OPID { get; set; }
        public int ClientID { get; set; }
        public int CP_id { get; set; }
        public int Acc_id { get; set; }
        public int Quantity { get; set; }
        public int ServiceGroup { get; set; }
        public int ServiceID { get; set; }
        public decimal SecAmount { get; set; }
        public string DetailInfo { get; set; }
        public int Status { get; set; }

    }
}
