namespace AccountingSystem.Web.Models
{
    public class CompanyInsertUpdateViewModel
    {
        public string Action { get; set; }
        public int CpId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CPerson { get; set; }
        public string Designation { get; set; }
        public int CompanyId { get; set; }
        public int DistrictId { get; set; }
        public int Type { get; set; }

    }
}
