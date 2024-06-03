namespace AccountingSystem.Models.AccountViewModels
{
    public class PaymentVarificationDataModelResponse
    {
        public int serialNo { get; set; }
        public int quotationID { get; set; }
        public string quotationNo { get; set; }
        public string uploadDateTime { get; set; }
        public int companyID { get; set; }
        public string companyName { get; set; }
        public string companyAddress { get; set; }
        public string companyAGM { get; set; }
        public string serviceName { get; set; }
        public object jobID { get; set; }
        public string jobTitle { get; set; }
        public int totalCost { get; set; }
        public string billerName { get; set; }
        public string billerEmail { get; set; }
        public string billerMobile { get; set; }
        public string paymentType { get; set; }
        public string referenceNo { get; set; }
        public string paymentDocumentLink { get; set; }
        public int verified { get; set; }
        public string InvoiceNo { get; set; }
        public string Comment { get; set; }
        public string ClientComment { get; set; }

        public decimal PaidAmount { get; set; }
        public decimal TDS { get; set; }
        public decimal VDS { get; set; }
    }

    public class ApiResponseModel
    {
        public string Message { get; set; }
        public string Error { get; set; }
        public List<PaymentVarificationDataModelResponse> Data { get; set; }
        public string pageNo { get; set; }
        public string pageSize { get; set; }
        public int totalRecords { get; set; }
    }
    public class VarificationResponseModel
    {
        public string Message { get; set; }
        public string Error { get; set; }
    }
}
