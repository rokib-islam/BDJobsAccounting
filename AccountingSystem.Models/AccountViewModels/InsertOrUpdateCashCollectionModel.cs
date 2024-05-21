using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class InsertOrUpdateCashCollectionModel
    {
        public int Type { get; set; }
        public int UserID { get; set; }
        public int CCollectionID { get; set; }
        public string InvoiceNo { get; set; }
        public float Cash { get; set; }
        public string ReceivedDate { get; set; }
        public int TNO { get; set; }
        public int InvoiceSchedulerID { get; set; }
        public int LedgerId { get; set; }
        public string ChequeDetails { get; set; }
        public string CompanyName { get; set; }
    }
}
