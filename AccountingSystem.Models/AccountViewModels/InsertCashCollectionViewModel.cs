namespace AccountingSystem.Models.AccountViewModels
{
    public class InsertCashCollectionViewModel
    {
        public string Type { get; set; }
        public string UserId { get; set; }
        public string InvoiceNo { get; set; }
        public string Cash { get; set; }
        public string Date { get; set; }
        public string Tno { get; set; }
        public string InvoiceSchedulerId { get; set; }
        public string LedgerId { get; set; }
        public string ChequeDetails { get; set; }
        public string CompanyName { get; set; }
        public int CashCollectionId { get; set; }
    }
}
