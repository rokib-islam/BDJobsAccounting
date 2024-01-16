namespace AccountingSystem.Web.Models
{
    public class GetJournalViewModel
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int IsPreview { get; set; }
        public string DateType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int LedgerId { get; set; }
        public string LedgerName { get; set; }
        public int CompanyId { get; set; }
        public int ApprovedBy { get; set; }
        public int PostedBy { get; set; }
        public int IsApproved { get; set; }
    }
}
