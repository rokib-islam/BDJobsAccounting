namespace AccountingSystem.Models.AccountViewModels
{
    public class LoadOnlineInvoiceModel
    {
        public string Status { get; set; }
        public string paymentMode { get; set; }
        public string ServiceName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
