namespace AccountingSystem.Models.AccountViewModels
{
    public class DeletedSalesViewModel
    {
        public string InvoiceNo { get; set; }
        public string ServiceName { get; set; }
        public double TotalPrice { get; set; }
        public double TotalVat { get; set; }
        public DateTime CancellationDate { get; set; }
        public string DeleteReason { get; set; }
        public int TotalRows { get; set; }
    }
}
