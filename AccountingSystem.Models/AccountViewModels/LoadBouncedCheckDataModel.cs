using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class LoadBouncedCheckDataModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int CId { get; set; }


        public string BouncedCheckDate { get; set; }
        public string Invoice_No { get; set; }
        public string BouncedCheckNo { get; set; }
        public float BouncedAmount { get; set; }
        public string Name { get; set; }
        public string BouncedNote { get; set; }
    }
}
