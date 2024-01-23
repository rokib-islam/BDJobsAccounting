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
    }


}
