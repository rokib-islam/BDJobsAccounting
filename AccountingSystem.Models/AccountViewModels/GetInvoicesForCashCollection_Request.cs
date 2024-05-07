using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class GetInvoicesForCashCollection_Request
    {
        public int CompanyId { get; set; }
        public int FullPayment { get; set; }
        public int Invalid { get; set; }
    }
}
