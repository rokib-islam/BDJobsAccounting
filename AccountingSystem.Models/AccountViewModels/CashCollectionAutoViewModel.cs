namespace AccountingSystem.Models.AccountViewModels
{
    public class CashCollectionAutoViewModel
    {
        public string InvoiceNo { get; set; }
        public double SalesPrice { get; set; }
        public double DiscountedPrice { get; set; }
        public int PaymentMode { get; set; }
        public string TransactionNo { get; set; }
        public string SDate { get; set; }
        public int CP_Id { get; set; }
        public int UserId { get; set; } = 0;
        public double TDS { get; set; }
        public double VDS { get; set; }
        public string jDate { get; set; } = "";
    }
    public class CashCollectionAutoReponse
    {
        public string Response { get; set; }

    }
}
