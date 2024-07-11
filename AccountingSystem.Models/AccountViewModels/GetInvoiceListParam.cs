namespace AccountingSystem.Models.AccountViewModels
{
    public class GetInvoiceListParam
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string ProductId { get; set; }
        public int Validity { get; set; }
        public string Operator { get; set; }
        public int FromDuration { get; set; }
        public int ToDuration { get; set; }
        public int FullPayment { get; set; }
        public int Blacklisted { get; set; }
        public string Order { get; set; }
        public string Location { get; set; }
        public int Salesperson { get; set; }
        public string FromRange { get; set; }
        public string ToRange { get; set; }
        public string Zone { get; set; }
    }
}
