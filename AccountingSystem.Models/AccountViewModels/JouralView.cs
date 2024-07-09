namespace AccountingSystem.Models.AccountViewModels
{
    public class JouralView
    {
        public int id { get; set; }
        public string SBName { get; set; }
        public string Description { get; set; }
        public double Debt { get; set; }
        public double Credit { get; set; }
        public string jDate { get; set; }
        public int sid { get; set; }
        public int jid { get; set; }
        public int UserID { get; set; }
        public string ApprovedBy { get; set; }
        public string Notify { get; set; }
        public int tno { get; set; }
        public string ApprovalDate { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string TotalRecords { get; set; }
        public string PostDate { get; set; }
    }
}