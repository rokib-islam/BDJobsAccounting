using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class ManualSalePostMonetizationModel
    {
        public string ServiceName { get; set; }
        public string ReceivedDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
