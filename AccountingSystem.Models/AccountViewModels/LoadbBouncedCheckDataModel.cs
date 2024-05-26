using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class LoadbBouncedCheckDataModel
    {
        public bool IsBounced { get; set; }
        public string BouncedCheckNo { get; set; }
        public DateTime BouncedCheckDate { get; set; }
        public float BouncedAmount { get; set; }
        public string BouncedNote { get; set; }
    }
}
