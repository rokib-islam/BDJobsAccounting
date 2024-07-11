namespace AccountingSystem.Models.AccountViewModels
{
    public class SalesReconciliationModel_Request
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int status { get; set; }
        public string company { get; set; }
    }
}
