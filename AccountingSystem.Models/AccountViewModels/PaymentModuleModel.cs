using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class PaymentModuleModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int PostingType { get; set; }
        public int VendorId { get; set; }
        public int ItemLedgerId { get; set; }
        public int PayableLedgerId { get; set; }
        public string BillReferenceNo { get; set; }
        public double BillAmount { get; set; }
        public int VATSectionId { get; set; }
        public string MushokNo { get; set; }
        public DateTime MushokDate { get; set; }
        public double VATAmount { get; set; }
        public double TotalBill { get; set; }
        public string Narration { get; set; }
        public int PaymentStatus { get; set; }
        public int VarifiedById { get; set; }
        public DateTime VarifiedDate { get; set; }
        public int EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
    }
}
