namespace AccountingSystem.Models.AccountViewModels
{
    public class OnlineInvoiceResponseModel
    {

        public string Response { get; set; }
        public int BulkService_ID { get; set; }
        public int JP_ID { get; set; }
        public int CP_ID { get; set; }
        public int Acc_Id { get; set; }
        public string InvoiceNo { get; set; }
        public string ChallanNo { get; set; }
        public string Quantity { get; set; }
        public string OneItemPrice { get; set; }
        public string TotalPrice { get; set; }
        public string TotalVat { get; set; }
        public string ServiceName { get; set; }
        public string Contact_Person { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Address { get; set; }
        public string VATRegNo { get; set; }
        public string PriceWithVat { get; set; }
        public string VatRate { get; set; }
        public string UserDegignation { get; set; }
        public string UserName { get; set; }
        public int IsCancel { get; set; }
        public string CreditNote { get; set; }
        public string InvoiceDesc { get; set; }
        public string InvoiceDescTax { get; set; }

    }
}
