using AccountingSystem.Models.AccountViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.Abstractions.Repository
{
    public interface IInvoiceRepository
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
        Task<List<LoadOnlineInvoiceResponseModel>> LoadOnlineInvoice(LoadOnlineInvoiceModel model);
        Task<CashCollectionAutoReponse> AutoCashCollection(CashCollectionAutoViewModel parameters);
        Task<int> CheckOrderIdCountAsync(string invoiceNo);
        Task UpdateOrderInvoiceTableAsync(string invoiceNo, string courierOrderId, int userId);

    }


}
