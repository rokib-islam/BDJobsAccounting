namespace AccountingSystem.Models.AccountViewModels
{
    public class InvoiceForOnlineJobViewModel
    {
        public int Id { get; set; }
        public int JpId { get; set; }
        public int jp_id { get; set; }
        public string Title { get; set; }
        public double SalesPrice { get; set; }
        public string InvoiceNo { get; set; }
        public string Invoice_No { get; set; }
        public string Submitted { get; set; }
        public int OpId { get; set; }
        public int AddType { get; set; }
        public int LedgerId { get; set; }
        public string BillingContact { get; set; }
        public double TotalAmount { get; set; }
        public double TAmount { get; set; }
        public int ServiceID { get; set; }
        public int CompanyID { get; set; }
        public int CP_ID { get; set; }
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public string Sdate { get; set; }
    }
}
