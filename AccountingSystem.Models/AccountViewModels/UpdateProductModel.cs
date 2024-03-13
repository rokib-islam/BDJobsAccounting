using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class UpdateProductModel
    {
        public string OldSid { get; set; }
        public string NewSid { get; set; }
        public string Tno { get; set; }
    }
}
