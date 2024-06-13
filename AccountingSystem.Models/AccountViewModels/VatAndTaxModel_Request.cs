using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class VatAndTaxModel_Request
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string CompanyName { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int UserID { get; set; }
        public int Type { get; set; }
    }
}
