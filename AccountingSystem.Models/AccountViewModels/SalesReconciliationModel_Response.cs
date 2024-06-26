using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class SalesReconciliationModel_Response
    {
        public int Acc_Id { get; set; }
        public int Jp_Id { get; set; }
        public int AddType { get; set; }
        public int RegionalJob { get; set; }
        public int BlueCollar { get; set; }
        public string P { get; set; }
        public string Deadline { get; set; }
        public string BillingContact { get; set; }
        public string Designation { get; set; }
        public int Cp_Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int City { get; set; }
        public string ContactName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
        public string Area { get; set; }
        public string TransId { get; set; }
        public string PaidBy { get; set; }
        public int OpId { get; set; }
        public int ServiceId { get; set; }
        public string JType { get; set; }
        public string ProviderPaymentRef { get; set; }
        public int Varified { get; set; }
        public string PublishDate { get; set; }
        public int Total { get; set; }
        public int PaymentMethod { get; set; }
        public float SalesPrice { get; set; }
        public float Tax { get; set; }
    }
}
