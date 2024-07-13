namespace AccountingSystem.Models.AccountViewModels
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }
        public string invoice_no { get; set; }
        public string CompanyName { get; set; }
        public decimal tamount { get; set; }
        public DateTime invsendDt { get; set; }
        public string Sent { get; set; }
        public string SendMode { get; set; }
        public bool Emailed { get; set; }
        public string FullPayment { get; set; }
        public int TotalInvoice { get; set; }
        public string SalesPersonName { get; set; }
        public string ProductName { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonPhone { get; set; }
        public int AgeignDay { get; set; }
        public int ProductWiseSequence { get; set; }
        public int LastInvDiff { get; set; }

    }
}
