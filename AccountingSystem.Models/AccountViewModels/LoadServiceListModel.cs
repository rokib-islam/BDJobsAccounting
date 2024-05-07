using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class LoadServiceListModel
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int VatRate { get; set; }
        public float UnitPrice { get; set; }
        public string Type { get; set; }
        public int TotalRecords { get; set; }
    }
}
