namespace AccountingSystem.Models.AccountViewModels
{
    public class loadOnlinePaymentVarificationDataModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Verified { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string paymentMode { get; set; }
    }
}
