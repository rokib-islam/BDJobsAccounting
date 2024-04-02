using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class LoadVatTaxCollectionDataModel_Response
    {
        public string VDS_CertificateNo { get; set; }
        public DateTime VDS_CertificateDate { get; set; }
        public string ChallanACCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string VATRegNo { get; set; }
        public string ServiceName { get; set; }
        public decimal TAmount { get; set; }
        public string ChallanNo { get; set; }
        public DateTime PostingDate { get; set; }
        public double ChallanAmount { get; set; }
    }
}
