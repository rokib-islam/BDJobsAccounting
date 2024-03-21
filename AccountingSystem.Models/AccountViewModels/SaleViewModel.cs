namespace AccountingSystem.Models.AccountViewModels
{
    public class SaleViewModel
    {
        public int Id { get; set; }
        public string Tno { get; set; }
        public string SbName { get; set; }
        public double Amount { get; set; }
        public double salesPrice { get; set; }
        public string Comment { get; set; }

        public string CompanyName { get; set; }
        public string name { get; set; }
        public string SDate { get; set; }

        public int TotalRecords { get; set; }
        public int TotalRecord { get; set; }

        public string FromDate { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string ToDate { get; set; }
        public double AccReceivale { get; set; }
        public bool IsPosted { get; set; }
        public bool Posted { get; set; }
        public int Duration { get; set; }
        public double Tax { get; set; }
        public int BillingContactId { get; set; }
        public int BillContactId { get; set; }
        public string RefNo { get; set; }
        public int TaxId { get; set; }
        public int SalesPerson { get; set; }
        public string InvoiceNo { get; set; }
        public string DeletedReason { get; set; }
        public double totalAmount { get; set; }
        public double DuesAmount { get; set; }
        public double totalVat { get; set; }
        public int IsCancel { get; set; }
    }
}
