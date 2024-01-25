namespace AccountingSystem.Models.AccountViewModels
{
    public class UpdateSalesJournal
    {
        public string Sid { get; set; }
        public string VatId { get; set; }
        public string Tno { get; set; }
        public string OldDuration { get; set; }
        public string NewDuration { get; set; }
        public string OldAmount { get; set; }
        public string NewAmount { get; set; }
        public string OldVatAmount { get; set; }
        public string NewVatAmount { get; set; }
        public string FromDate { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
    }
}
