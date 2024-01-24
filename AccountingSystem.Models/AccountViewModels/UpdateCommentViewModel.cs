namespace AccountingSystem.Models.AccountViewModels
{
    public class UpdateCommentViewModel
    {
        public string Action { get; set; }
        public string InvSchId { get; set; }
        public string InvoiceId { get; set; }
        public string Amount { get; set; }
        public string Comments { get; set; }
        public string SendDate { get; set; }
    }
}
