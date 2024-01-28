namespace AccountingSystem.Models.AccountDbModels
{
    public class Invoice
    {
        public int Id { get; set; }
        public int Cid { get; set; }
        public string InvoiceNo { get; set; }
        public string invoice_no { get; set; }
        public double TAmount { get; set; }
        public string Comments { get; set; }
        public string InvSendDt { get; set; }
        public string Sent { get; set; }
        public string SendingDt { get; set; }
        public string SendMode { get; set; }
        public string FullPayment { get; set; }
        public string PostedNature { get; set; }
        public int Emailed { get; set; }
        public int TotalPrinted { get; set; }
        public string Invalid { get; set; }
        public string UploadedPaymentStatus { get; set; }
        public string MoneyRecieptNo { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string SalesPerson { get; set; }
        public int TotalInvoices { get; set; }
        public string TaxVatCollectionStatus { get; set; }
        public string AccPersonMail { get; set; }
        public string SalesPersonName { get; set; }
        public int CompanyId { get; set; }
    }
}
