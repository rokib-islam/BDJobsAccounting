using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class VatAndTaxModel_Response
    {
        public int SLNO { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string ContactPerson { get; set; }

        public double AIT { get; set; }

        public double VAT { get; set; }

        public double Total { get; set; }

        public int TotalRecord { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string UserName { get; set; }

        public string Designation { get; set; }

        public int EmpID { get; set; }
        public string AccPersonMail { get; set; }
        public string AccPersonContactNo { get; set; }
        public string SignatureImage { get; set; }
        public string LastMailSendDate { get; set; }
        public string Period { get; set; }
        public string LastPeriod { get; set; }
        public bool AutoMail { get; set; }
    }
}
