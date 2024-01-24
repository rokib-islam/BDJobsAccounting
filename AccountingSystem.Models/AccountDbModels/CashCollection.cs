﻿namespace AccountingSystem.Models.AccountDbModels
{
    public class CashCollection
    {
        public int Id { get; set; }
        public double Cash { get; set; }
        public double SalesTax { get; set; }
        public string ReceivedDate { get; set; }
        public bool Posted { get; set; }
        public string PaymentType { get; set; }
        public string ChequeDetails { get; set; }
        public bool BadDebt { get; set; }
        public int BankId { get; set; }
    }
}
