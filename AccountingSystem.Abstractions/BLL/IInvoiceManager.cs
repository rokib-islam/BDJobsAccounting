using AccountingSystem.Models.AccountViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.Abstractions.BLL
{
    public interface IInvoiceManager
    {
        Task<List<InvoiceForOnlineJobViewModel>> GetInvoices(int cpId, string sDate, int ledgerId);
        Task<object> UploadInvoiceOnline(int cpId, string invoiceNo, int serviceNo, string invSendDt, string billingContact, string price, string opId, string jpIdList, int serviceID, int companyID, string companyName, string saleDate);
        Task<string> GetInvSendDt(string invoiceNo);
        Task<string> UpdateInvoice(string invoiceNo);
        Task<string> SaveInvoice([FromBody] SaveInvoiceViewModel data);
        Task<bool> CheckInvoiceNo(string invoiceNo);
        Task<string> UpdateDeleteComments(UpdateCommentViewModel data);
        Task<IEnumerable<InvoiceViewModel>> GetInvoicesForCashCollectionAsync(string query);
        Task<string> PostToOnlineAsync(string postType, string invoiceNo, string invoiceId);
        Task<List<ProductForInvoice>> Getproducts(string cId, int type);
        Task<string> GenerateInvoiceNumberAsync(string cId, string issueDate);
        Task<List<InvoiceViewModel>> GetProductsDetails(string id);
        Task<string> DeleteUndeleteInvoice(int invoiceId, bool invalid);
        Task<string> UpdateAmount(string invoiceNo, decimal amount);
        Task<List<InvoiceViewModel>> GetInvoicesAsync(GetInvoiceListParam parameters);
        Task<OnlineInvoiceResponseModel> OnlineInvcoie(OnlineInvoiceRequestModel parameters);
        Task<OnlineInvoiceResponseModel> OnlineInvocie_For_Payment_Doc(OnlineInvoiceRequestModel parameters);
        Task<OnlineInvoiceResponseModel> OnlineInvocie_For_Payment_Doc_test(OnlineInvoiceRequestModel parameters);
        Task<OnlineInvoiceResponseModel> OnlineInvcoietest(OnlineInvoiceRequestModel parameters);
        Task<List<LoadOnlineInvoiceResponseModel>> LoadOnlineInvoice(LoadOnlineInvoiceModel model);
        Task<CashCollectionAutoReponse> AutoCashCollection(CashCollectionAutoViewModel parameters);
        Task<CashCollectionAutoReponse> AutoCashCollection_For_Payment_Doc(CashCollectionAutoViewModel parameters);
        Task<CashCollectionAutoReponse> AutoCashCollection_For_Payment_Doc_test(CashCollectionAutoViewModel parameters);
        Task<CashCollectionAutoReponse> AutoCashCollectiontest(CashCollectionAutoViewModel parameters);
        Task<string> CheckOrderIdCountAsync(string invoiceNo);
        Task UpdateOrderInvoiceTableAsync(string invoiceNo, string courierOrderId, int userId);
        Task<List<LoadBouncedCheckDataModel>> LoadBouncedCheckData(LoadBouncedCheckDataModel model);
        Task<string> UpdateBouncedChequeData(UpdateBouncedChequeDataModel data);
        Task<List<LoadbBouncedCheckDataModel>> LoadbBouncedCheckData(string invoiceNo);
        Task<OnlineInvoiceResponseModel> CreatePaybaleByJobPost(OnlineInvoiceRequestModel parameters);
        Task<CashCollectionAutoReponse> AutoCashCollection_Multiple_Invoice(CashCollectionAutoViewModel parameters);
        Task<OnlineInvoiceResponseModel> CMPackageAutoBill(CmPackageViewModel parameters);

    }
}
