namespace AccountingSystem.Models.AccountViewModels
{
    public class LoadOnlineInvoiceResponseModel
    {
        public decimal Price { get; set; }
        public decimal Vat { get; set; }
        public string Status { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime PostDate { get; set; }
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public string TransactionNo { get; set; }
        public int JP_ID { get; set; }
        public string paymentMode { get; set; }
        public string ServiceName { get; set; }
    }
}
