using System.ComponentModel.DataAnnotations;

namespace AccountingSystem.Models.AccountViewModels
{
    public class LedgerViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Group Name is required")]
        public string GroupName { get; set; }
        public string Under { get; set; }
        [Required(ErrorMessage = "Main Group is required")]
        public string MaingroupName { get; set; }
        public int LevelNo { get; set; }
        public double OpeningBalance { get; set; }
        public DateTime OpeningDate { get; set; }
        public bool LedgerAcc { get; set; }
        public double Balance { get; set; }
        public string Account { get; set; }

        public string SBName { get; set; }
        public string LadgerName { get; set; }
        public string MGroup { get; set; }

        public double Total { get; set; }
        public int ServiceID { get; set; }
        public string Title { get; set; }
        public double VatRate { get; set; }
    }
}
