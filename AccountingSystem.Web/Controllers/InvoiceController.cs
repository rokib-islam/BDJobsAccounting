using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Models.AccountViewModels;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> UploadInvoicesOnline(int? cpId, string invoiceNo, int serviceNo, string billingContact, string price, string opId, string jpIdList, int serviceID, int companyID, string companyName, string saleDate)
        {
            var invSendDt = await _InvoiceManager.GetInvSendDt(invoiceNo);
            var cid = cpId ?? 0;
            var results = await _InvoiceManager.UploadInvoiceOnline(cid, invoiceNo, serviceNo, invSendDt, billingContact, price, opId, jpIdList, serviceID, companyID, companyName, saleDate);

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




    }
}
