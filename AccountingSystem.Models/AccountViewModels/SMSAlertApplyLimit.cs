namespace AccountingSystem.Models.AccountViewModels
{
    public class SMSAlertApplyLimit
    {
        public int ServiceGroup { get; set; }
        public int OPID { get; set; }
        public string ServiceName { get; set; }
        public int P_ID { get; set; }
        public int Quantity { get; set; }
        public decimal PaidAmount { get; set; }
        public string TransID { get; set; }
        public string TransDate { get; set; }
        public string PaidBy { get; set; }
        public int ServiceID { get; set; }
        public decimal Amount { get; set; }
        public string DetailInfo { get; set; }
    }
}
