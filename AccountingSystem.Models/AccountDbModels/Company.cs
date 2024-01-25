using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AccountingSystem.Models.AccountDbModels
{
    [Keyless]
    public class Company
    {
        public int Id { get; set; }
        [DisplayName("Company Name")]
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string ContactPerson { get; set; }
        public string Designation { get; set; }
        public double Balance { get; set; }
        public bool BlackListed { get; set; }

        [DisplayName("Online Id")]
        [DataType(DataType.Text)]
        public int CP_Id { get; set; }

        [DisplayName("Acc. Person")]
        public string AccContactName { get; set; }
        public string VatRegNo { get; set; }
        public string VatRegAdd { get; set; }

        public string AccCreatedDate { get; set; }

        public string DistrictId { get; set; }

        public int BankId { get; set; }

        [DisplayName("Vat Challan Name")]
        public string VatChallanName { get; set; }

        [DisplayName("Acc. Person Mail")]
        public string AccPersonMail { get; set; }

        [DisplayName("Acc. Person Contact No")]
        public string AccPersonContactNo { get; set; }
    }
}
