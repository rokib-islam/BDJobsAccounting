using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class UpdateBouncedChequeDataModel
    {
        public bool IsBounced { get; set; }
        public string ChequeNo { get; set; }
        public string BouncedDate { get; set; }
        public float BouncedAmount { get; set; }
        public string BouncedNote { get; set; }
        public string InvoiceNo { get; set; }
    }
}
