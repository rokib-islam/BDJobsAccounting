using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class Acknowledgement_GrossSalary_TA_Model
    {
        public int ID { get; set; }
        public string AcknowledgementNo { get; set; }
        public string ReturnYear { get; set; }
        public int EmployeeId { get; set; }
        public int VendorId { get; set; }

        public float GrossSalary { get; set; }
        public string GrossSalaryEffectiveDate { get; set; }

        public float TA { get; set; }
        public string TAEffectiveDate { get; set; }
    }
}
