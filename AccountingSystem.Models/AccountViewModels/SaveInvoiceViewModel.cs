namespace AccountingSystem.Models.AccountViewModels
{
    public class SaveInvoiceViewModel
    {
        public int UserId { get; set; }
        public string Action { get; set; }
        public string CId { get; set; }
        public string Invoice { get; set; }
        public string IssueDate { get; set; }
        public string TotalPrice { get; set; }
        public string IdList { get; set; }
        public string InvoiceId { get; set; }
    }
}
