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
        public async Task<IActionResult> GetSalesPersons(int productID)
        {
            var returnValue = await _SaleManager.GetSalesPersonsAsync(productID);
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

    }
}
