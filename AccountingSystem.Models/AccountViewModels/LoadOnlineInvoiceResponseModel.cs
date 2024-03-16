﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class LoadOnlineInvoiceResponseModel
    {
        public decimal Price { get; set; }
        public decimal Vat { get; set; }
        public string Status { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime PostDate { get; set; }
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public int JP_ID { get; set; }
    }
}
