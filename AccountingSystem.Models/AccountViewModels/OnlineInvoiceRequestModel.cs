namespace AccountingSystem.Models.AccountViewModels
{
    public class OnlineInvoiceRequestModel
    {
        public int Acc_Id { get; set; }
        public int Jp_Id { get; set; }
        public int AddType { get; set; }
        public int Regional { get; set; }
        public int BlueCollar { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal Tax { get; set; }
        public string SDate { get; set; }
        public string EDate { get; set; }
        public string SalesPersonName { get; set; }
        public string BillingContact { get; set; }
        public string Designation { get; set; }
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Cp_Id { get; set; }
        public int DistrictId { get; set; }
        public string BINNo { get; set; }
        public string JType { get; set; }
        public string TransactionNo { get; set; }
        public int PaymentMethod { get; set; }
        public int UserId { get; set; } = 0;
        public decimal TDS { get; set; }
        public decimal VDS { get; set; }
        public string jDate { get; set; }

    }
}
