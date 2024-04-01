using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class VarifyOrReject
    {
        public int QID { get; set; }
        public int verified { get; set; }
        public int verifiedBy { get; set; }
    }
}
