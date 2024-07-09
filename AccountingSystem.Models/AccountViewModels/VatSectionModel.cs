using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class VatSectionModel
    {
        public int Id { get; set; }
        public string VatSectionName { get; set; }
        public int VatRate { get; set; }
    }
}
