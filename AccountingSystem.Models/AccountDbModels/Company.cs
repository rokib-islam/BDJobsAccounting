using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AccountingSystem.Models.AccountDbModels
{
    [Keyless]
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Contact_Person { get; set; }
        public string Designation { get; set; }
        public double Balance { get; set; }
        public bool BlackListed { get; set; }
        public int CP_ID { get; set; }
        public string AccContactName { get; set; }
        public string VATRegNo { get; set; }
        public string VATRegAdd { get; set; }
        public short? DistrictID { get; set; }
        public int? UpazilaID { get; set; }
        public short? BankID { get; set; }
        public string VatChallanName { get; set; }
        public DateTime? PostedOn { get; set; }
        public string AccPersonMail { get; set; }
        public string AccPersonContactNo { get; set; }
    }
}
