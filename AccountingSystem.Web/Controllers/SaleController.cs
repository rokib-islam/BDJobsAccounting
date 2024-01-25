using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Models.AccountViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AccountingSystem.Web.Controllers
{
    public class SaleController : Controller
    {
        private readonly ISaleManager _SaleManager;

        public SaleController(ISaleManager SaleManager)
        {
            _SaleManager = SaleManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> FixDownloadIssue()
        {
            await _SaleManager.FixDownloadIssue();

            return Json(true);
        }
        public async Task<IActionResult> GetOnlineJobList(string CName, int Verified, int LedgerID)
        {
            var data = await _SaleManager.GetOnlineJobList(CName, Verified, LedgerID);

            return Json(data);
        }
        public async Task<IActionResult> GetJobs(int cpId, string date, int adType, int adRegion)
        {
            var data = await _SaleManager.GetJobs(cpId, date, adType, adRegion);

            return Json(data);
        }
        public async Task<IActionResult> DeleteOnlineJob(int jpId)
        {
            await _SaleManager.DeleteOnlineJob(jpId);

            return Json(true);
        }
        public async Task<IActionResult> DownLoadOnlineJobs(string fromDate, string toDate, int PNPL)
        {
            var returnValue = 0;
            var isUploaded = _SaleManager.IsAllUploaded();
            if (!await isUploaded)
            {
                returnValue = 1;
            }
            else
            {

                returnValue = await _SaleManager.DownloadJobs(fromDate, toDate, PNPL);

            }
            return Json(returnValue);
        }

        public IActionResult OnlineJobs()
        {
            ClaimsPrincipal claimusers = HttpContext.User;
            if (claimusers.Identity.IsAuthenticated)
                return View();

            else
                return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> GetSalesPersonsByProductID(int productID)
        {
            var returnValue = await _SaleManager.GetSalesPersons(productID);
            return Json(returnValue);
        }
        public async Task<IActionResult> Save([FromBody] SaveSalesDataViewModel Data)
        {
            var result = await _SaleManager.SaveSale(Data);
            return Json(result);
        }
        public async Task<IActionResult> CheckOnlineJobs(string tnolist, string cId)
        {
            var result = await _SaleManager.CheckOnlineJobsAsync(tnolist, cId);
            return Json(new { Online = result[0], Total = result[1] });
        }
        public async Task<IActionResult> GetSalesInfo(string InvoiceNo)
        {
            var result = await _SaleManager.GetSalesInfoAsync(InvoiceNo);

            return Json(result);
        }
        public async Task<IActionResult> DownloadSMSAlert(int serviceId, string fDate, string tDate)
        {
            var result = await _SaleManager.DownloadSMSAlertAsync(serviceId, fDate, tDate);

            return Json(result);
        }
        public async Task<IActionResult> PostSMSAlertApplyLimitSalePosting([FromBody] PostSMSAlertApplyLimitSale Data)
        {
            var result = await _SaleManager.PostSMSAlertApplyLimitSalePosting(Data);

            return Json(result);
        }
        public async Task<IActionResult> GetSMSAlertApplyLimitForOnlinePost(string OPIDs)
        {
            var result = await _SaleManager.GetSMSAlertApplyLimitForOnlinePost(OPIDs);

            return Json(result);
        }
        public async Task<IActionResult> PostSMSAlertApplyLimitToOnline(string OPIDs, int CMorJS, int Type)
        {
            var result = await _SaleManager.PostSMSAlertApplyLimitToOnline(OPIDs, CMorJS, Type);

            return Json(result);
        }
        public async Task<IActionResult> GetSMSAlertApplyLimit(GetSMSApplyLimit Data)
        {
            var result = await _SaleManager.GetSMSAlertApplyLimit(Data);

            return Json(result);
        }
        public async Task<IActionResult> CheckJobTitle(int productId)
        {
            var result = await _SaleManager.CheckJobTitle(productId);

            return Json(result);
        }

    }
}
