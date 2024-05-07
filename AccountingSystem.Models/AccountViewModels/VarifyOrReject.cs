namespace AccountingSystem.Models.AccountViewModels
{
    public class VarifyOrReject
    {
        public int QID { get; set; }
        public int verified { get; set; }
        public int verifiedBy { get; set; }
        public string Comment { get; set; }
    }
}
