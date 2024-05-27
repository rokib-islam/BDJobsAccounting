using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class LoadPfPaymentDataModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int EmployeeId { get; set; }
    }
}
