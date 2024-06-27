using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class SalesReconciliationModel_Request
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
