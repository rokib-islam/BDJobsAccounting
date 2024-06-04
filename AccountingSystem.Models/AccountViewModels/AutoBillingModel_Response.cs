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

    }
}
