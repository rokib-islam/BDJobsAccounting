namespace AccountingSystem.Models.AccountViewModels
{
    public class ChallandetailsForOnlinePosting
    {
        public string InvoiceNo { get; set; }
        public string ChallanNo { get; set; }
        public int Quantity { get; set; }
        public decimal OneItemPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalPriceAll { get; set; }
        public decimal TotalVat { get; set; }
        public decimal TotalVatAll { get; set; }
        public string ServiceName { get; set; }
        public string Contact_Person { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Address { get; set; }
        public string VATRegNo { get; set; }
        public decimal PriceWithVat { get; set; }
        public decimal priceWithVatAll { get; set; }
        public decimal VatRate { get; set; }
        public string UserDegignation { get; set; }
        public string UserName { get; set; }
        public string ServiceType { get; set; }
    }
}
