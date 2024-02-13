using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class UploadInvoicesOnlineModel
    {
        public int? CpId { get; set; }
        public string InvoiceNo { get; set; }
        public int ServiceNo { get; set; }
        public string BillingContact { get; set; }
        public string Price { get; set; }
        public string OpId { get; set; }
        public string JpIdList { get; set; }
        public int ServiceID { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string SaleDate { get; set; }
    }
}
