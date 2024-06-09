namespace AccountingSystem.Models.AccountViewModels
{
    public class CVAndSMSPurchesModel
    {
        public int OPID { get; set; }
        public int ClientID { get; set; }
        public int CP_ID { get; set; }
        public int Acc_Id { get; set; }
        public int Quantity { get; set; }
        public int ServiceGroup { get; set; }
        public int ServiceID { get; set; }
        public string ReceivedDate { get; set; }
        public string TransDate { get; set; }
        public double PaidAmount { get; set; }
        public double Amount { get; set; }
        public string DetailInfo { get; set; }
        public string TransID { get; set; }
        public string PaidBy { get; set; }
        public string ServiceName { get; set; }
    }
}
