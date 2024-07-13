namespace AccountingSystem.Models.AccountViewModels
{
    public class CmPackageViewModel
    {
        public int BulkService_ID { get; set; }
        public int CP_ID { get; set; }
        public int Acc_Id { get; set; }
        public string Price { get; set; }
        public string Vat { get; set; }
        public string ServiceType { get; set; }
        public string Quantity { get; set; }
        public int Duration { get; set; }
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
        public int OnlineDistrictId { get; set; }
        public string BINNo { get; set; }
        public string TransactionNo { get; set; }
        public int PaymentMode { get; set; }
    }
}
