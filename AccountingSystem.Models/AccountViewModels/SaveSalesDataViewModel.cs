using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class SaveSalesDataViewModel
    {
        public int UserId { get; set; }
        public int CId { get; set; }
        public int PCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string JournalDate { get; set; }
        public double SalesPrice { get; set; }
        public string BillingPerson { get; set; }
        public string Designation { get; set; }
        public string Comment { get; set; }
        public int Duration { get; set; }
        public int NoOfInvoice { get; set; }
        public string RefNo { get; set; }
        public string TypeId { get; set; }
        public double Vat { get; set; }
        public int JpId { get; set; }
        public string JobTitle { get; set; }
        public string WorkshopDate { get; set; }
        public int SPerson { get; set; }
    }
}
