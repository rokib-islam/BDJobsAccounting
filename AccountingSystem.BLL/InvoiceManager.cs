using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Abstractions.Repository;
using AccountingSystem.Models.AccountViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.BLL
{
    public class InvoiceManager : IInvoiceManager
    {
        private readonly IInvoiceRepository _repository;
        public InvoiceManager(IInvoiceRepository repository) //: base(repository)
        {
            _repository = repository;
        }

        public Task<bool> CheckInvoiceNo(string invoiceNo)
        {
            return _repository.CheckInvoiceNo(invoiceNo);
        }

        public Task<string> DeleteUndeleteInvoice(int invoiceId, bool invalid)
        {
            return _repository.DeleteUndeleteInvoice(invoiceId, invalid);
        }

        public Task<string> GenerateInvoiceNumberAsync(string cId, string issueDate)
        {
            return _repository.GenerateInvoiceNumberAsync(cId, issueDate);
        }

        public async Task<List<InvoiceForOnlineJobViewModel>> GetInvoices(int cpId, string sDate, int ledgerId)
        {
            return await _repository.GetInvoices(cpId, sDate, ledgerId);
        }

        public Task<List<InvoiceViewModel>> GetInvoicesAsync(GetInvoiceListParam parameters)
        {
            return _repository.GetInvoicesAsync(parameters);
        }

        public Task<IEnumerable<InvoiceViewModel>> GetInvoicesForCashCollectionAsync(string query)
        {
            return _repository.GetInvoicesForCashCollectionAsync(query);
        }

        public async Task<string> GetInvSendDt(string invoiceNo)
        {
            return await _repository.GetInvSendDt(invoiceNo);
        }

        public Task<List<ProductForInvoice>> Getproducts(string cId, int type)
        {
            return _repository.Getproducts(cId, type);
        }

        public Task<List<InvoiceViewModel>> GetProductsDetails(string id)
        {
            return _repository.GetProductsDetails(id);
        }

        public Task<OnlineInvoiceResponseModel> OnlineInvcoie(OnlineInvoiceRequestModel parameters)
        {
            return _repository.OnlineInvcoie(parameters);
        }

        public Task<string> PostToOnlineAsync(string postType, string invoiceNo, string invoiceId)
        {
            return _repository.PostToOnlineAsync(postType, invoiceNo, invoiceId);
        }

        public Task<string> SaveInvoice([FromBody] SaveInvoiceViewModel data)
        {
            return _repository.SaveInvoice(data);
        }

        public Task<string> UpdateAmount(string invoiceNo, decimal amount)
        {
            return _repository.UpdateAmount(invoiceNo, amount);
        }

        public Task<string> UpdateDeleteComments(UpdateCommentViewModel data)
        {
            return _repository.UpdateDeleteComments(data);
        }

        public async Task<string> UpdateInvoice(string invoiceNo)
        {
            return await _repository.UpdateInvoice(invoiceNo);
        }

        public async Task<object> UploadInvoiceOnline(int cpId, string invoiceNo, int serviceNo, string invSendDt, string billingContact, string price, string opId, string jpIdList, int serviceID, int companyID, string companyName, string saleDate)
        {
            return await _repository.UploadInvoiceOnline(cpId, invoiceNo, serviceNo, invSendDt, billingContact, price, opId, jpIdList, serviceID, companyID, companyName, saleDate);
        }

        public async Task<List<LoadOnlineInvoiceResponseModel>> LoadOnlineInvoice(LoadOnlineInvoiceModel model)
        {
            return await _repository.LoadOnlineInvoice(model);
        }

        public async Task<CashCollectionAutoReponse> AutoCashCollection(CashCollectionAutoViewModel parameters)
        {
            return await _repository.AutoCashCollection(parameters);
        }

        public async Task<int> CheckOrderIdCountAsync(string invoiceNo)
        {
            return await _repository.CheckOrderIdCountAsync(invoiceNo);
        }

        public async Task UpdateOrderInvoiceTableAsync(string invoiceNo, string courierOrderId)
        {
            //return await _repository.UpdateOrderInvoiceTableAsync(invoiceNo, courierOrderId);
            await _repository.UpdateOrderInvoiceTableAsync(invoiceNo, courierOrderId);
        }
    }
}
