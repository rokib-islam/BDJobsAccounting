namespace AccountingSystem.Models.AccountViewModels
{
    public class JobListV2ViewModel
    {
        public int Serial { get; set; }
        public int CP_ID { get; set; }
        public int Acc_Id { get; set; }
        public string CName { get; set; }
        public int JobPosted { get; set; }
        public int TotalJobPosted { get; set; }
        public int OPID { get; set; }
        public string AddType { get; set; }
        public string CompanyStatus { get; set; }
        public string PostingDate { get; set; }
        public int Region { get; set; }
        public string VerifiedCompany { get; set; }
        public string JType { get; set; }
    }
}
