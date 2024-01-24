namespace AccountingSystem.Models.AccountViewModels
{
    public class GetSMSApplyLimit
    {
        public int ServiceGroup { get; set; }
        public int ServiceID { get; set; }
        public string PaymentMethod { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
