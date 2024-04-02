using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class GetJournalsForTrialBalanceModel
    {
        public string PageNo { get; set; }
        public string PageSize { get; set; }
        public string Tno { get; set; }
        public string FromDate { get; set; }
        public string EndDate { get; set; }
    }
}
