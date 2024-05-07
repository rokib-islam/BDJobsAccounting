using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class LoadVatTaxCollectionDataModel_Request
    {
        public int CId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
