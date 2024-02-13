using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Models.AccountViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AccountingSystem.Web.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IInvoiceManager _InvoiceManager;

        public InvoiceController(IInvoiceManager InvoiceManagerManager)
        {
            _InvoiceManager = InvoiceManagerManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetInvoices(int cpId, string sDate, int ledgerId)
        {
            var data = await _InvoiceManager.GetInvoices(cpId, sDate, ledgerId);

            return Json(data);
        }
        //public async Task<IActionResult> UploadInvoicesOnline(int? cpId, string invoiceNo, int serviceNo, string billingContact, string price, string opId, string jpIdList, int serviceID, int companyID, string companyName, string saleDate)
        //{
        //    var invSendDt = await _InvoiceManager.GetInvSendDt(invoiceNo);
        //    var cid = cpId ?? 0;
        //    var results = await _InvoiceManager.UploadInvoiceOnline(cid, invoiceNo, serviceNo, invSendDt, billingContact, price, opId, jpIdList, serviceID, companyID, companyName, saleDate);

        //    return Json(results);
        //}

        [HttpPost]
        public async Task<IActionResult> UploadInvoicesOnline([FromBody] UploadInvoicesOnlineModel model)
        {
            var invSendDt = await _InvoiceManager.GetInvSendDt(model.InvoiceNo);
            var cid = model.CpId ?? 0;
            var results = await _InvoiceManager.UploadInvoiceOnline(cid, model.InvoiceNo, model.ServiceNo, invSendDt, model.BillingContact, model.Price, model.OpId, model.JpIdList, model.ServiceID, model.CompanyID, model.CompanyName, model.SaleDate);

            return Json(results);
        }
        public async Task<IActionResult> UpdateInvoice(string invoiceNo)
        {
            var results = await _InvoiceManager.UpdateInvoice(invoiceNo);

            return Json(results);
        }
        public async Task<IActionResult> SaveInvoice([FromBody] SaveInvoiceViewModel data)
        {
            var results = await _InvoiceManager.SaveInvoice(data);

            return Json(results);
        }
        public async Task<IActionResult> UpdateDeleteComments([FromBody] UpdateCommentViewModel data)
        {
            var results = await _InvoiceManager.UpdateDeleteComments(data);

            return Json(results);
        }
        public async Task<IActionResult> GetInvoicesForCashCollection(int CompanyId, int FullPayment, int Invalid)
        {
            var results = await _InvoiceManager.GetInvoicesForCashCollectionAsync(CompanyId, FullPayment, Invalid);

            return Json(results);
        }
        public async Task<IActionResult> PostToOnline(string postType, string invoiceNo, string invoiceId)
        {
            var results = await _InvoiceManager.PostToOnlineAsync(postType, invoiceNo, invoiceId);

            return Json(results);
        }
        public async Task<IActionResult> GetProductsForInvoice(string cId, int type)
        {
            var results = await _InvoiceManager.Getproducts(cId, type);

            return Json(results);
        }
        public async Task<IActionResult> GenerateInvoiceNumber(string cId, string issueDate)
        {
            var results = await _InvoiceManager.GenerateInvoiceNumberAsync(cId, issueDate);

            return Json(results);
        }
        public async Task<IActionResult> GetProductsDetails(string id)
        {
            var results = await _InvoiceManager.GetProductsDetails(id);

            return Json(results);
        }

        public IActionResult MakeInvoice(string companyId)
        {
            ClaimsPrincipal claimsPrincipal = HttpContext.User;
            if (claimsPrincipal.Identity.IsAuthenticated)
            {
                ViewBag.OnlineCompanyId = companyId;
                return View();
            }

            else
                return RedirectToAction("Index", "Home");
        }

       
        public Task<bool> CheckInvoiceNo(string invoiceNo)
        {
            var results = _InvoiceManager.CheckInvoiceNo(invoiceNo);

            return results;
        }

        public async Task<IActionResult> DeleteUndeleteInvoice(int invoiceId, bool invalid)
        {
            var results = await _InvoiceManager.DeleteUndeleteInvoice(invoiceId, invalid);

            return Json(results);
        }
        public async Task<IActionResult> UpdateAmount(string invoiceNo, decimal amount)
        {
            var results = await _InvoiceManager.UpdateAmount(invoiceNo, amount);

            return Json(results);
        }
        public async Task<IActionResult> GetInvoiceList(GetInvoiceListParam parameters)
        {
            var results = await _InvoiceManager.GetInvoicesAsync(parameters);

            return Json(results);
        }

    }
}
