using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class InsertProvidentFundPaymentModel
    {
        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public int EmployeeId { get; set; }
        public int LedgerId { get; set; }
        public string PaymentRef { get; set; }
        public float EmpContribution_pay { get; set; }
        public float EmpContribution_adj { get; set; }
        public float ComContribution_pay { get; set; }
        public float ComContribution_adj { get; set; }
        public float EmpProfitCont_pay { get; set; }
        public float EmpProfitCont_adj { get; set; }
        public float ComProfitCont_pay { get; set; }
        public float ComProfitCont_adj { get; set; }
        public int UserId { get; set; }
    }
}
