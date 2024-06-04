﻿using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Models.AccountViewModels;
using Hangfire;
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
        [HttpPost]
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
        public async Task<IActionResult> GetSales(string pageNo, string pageSize, int cId, int tno)
        {
            var result = await _SaleManager.GetSales(pageNo, pageSize, cId, tno);

            return Json(result);
        }
        public async Task<IActionResult> GetDeletedSales(string pageNo, string pageSize, int cId)
        {
            var result = await _SaleManager.GetDeletedSales(pageNo, pageSize, cId);

            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductModel model)
        {
            var result = await _SaleManager.UpdateSaleProduct(model);

            return Json(result);
        }
        public async Task<IActionResult> DeleteSale(int tno, string deleteReason, int creditNote, DateTime deleteDate)
        {
            var result = await _SaleManager.DeleteSale(tno, deleteReason, creditNote, deleteDate);
            return Json(result);
        }
        public async Task<IActionResult> MakeJournalOfSale([FromBody] MakeJournalOfSales saleInfo)
        {
            var result = await _SaleManager.MakeJournalOfSale(saleInfo);
            return Json(result);
        }
        public async Task<IActionResult> GetNumberOfId(string tno)
        {
            var result = await _SaleManager.GetNumberOfId(tno);
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSaleInfo([FromBody] UpdateSalesInfo salesInfo)
        {
            var result = await _SaleManager.UpdateSaleInfoAsync(salesInfo);
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSalePosted([FromBody] MakeJournalOfSales saleInfo)
        {
            var result = await _SaleManager.UpdateSalePosted(saleInfo);
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSaleContactPersonAndRefNo([FromBody] UpdateSaleContactPersonAndRefNoModel model)
        {
            var result = await _SaleManager.UpdateSaleContactPersonAndRefNo(model);
            return Json(result);
        }

        public IActionResult NewSale(int? onlinejobId, int? onlineLedgerId, int? companyid)
        {
            ClaimsPrincipal claimsPrincipal = HttpContext.User;
            if (claimsPrincipal.Identity.IsAuthenticated)
            {
                ViewBag.OnlineJobId = onlinejobId;
                ViewBag.OnlineLedgerId = onlineLedgerId;
                ViewBag.OnlineCompanyId = companyid;
                return View();
            }

            else
                return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> GetSalesPersonListByKey(string startingKey)
        {
            var result = await _SaleManager.GetSalesPersonListByKey(startingKey);
            return Json(result);
        }


        public IActionResult EditSale()
        {
            ClaimsPrincipal claimusers = HttpContext.User;
            if (claimusers.Identity.IsAuthenticated)
                return View();

            else
                return RedirectToAction("Index", "Home");
        }

        #region candidate monetization auto billing system Using Hangfire
        public async Task DownloadCandidateMonetizationAsync()
        {
            var result = await _SaleManager.DownloadCandidateMonetizationAsync();
        }
        public async Task SalePostMonetizationBasicAsync(string serviceName)
        {
            var result = await _SaleManager.PostSMSAlertApplyLimitSalePostingNew(serviceName);

        }

        public void DownloadCandidateMonetizationJob()
        {
            RecurringJob.AddOrUpdate(
                "DownloadCandidateMonetizationJob",
                () => DownloadCandidateMonetizationAsync().GetAwaiter().GetResult(),
                //Cron.Hourly
                "0 */2 * * *"
            );
        }

        public void SalesPostingMonetizationJobs()
        {
            RecurringJob.AddOrUpdate(
                "Candidate_Monetization_Sequential_Sale_Posting",
                () => RunMonetizationJobsSequentially().GetAwaiter().GetResult(),
                "0 */2 * * *" // Adjust the cron expression as needed
            );
        }

        public async Task RunMonetizationJobsSequentially()
        {
            var count = 0;

            count = await _SaleManager.SMSAlertApplyLimitCountForBilling("Candidate Monetization-Basic");
            if (count > 100)
            {
                await SalePostMonetizationBasicAsync("Candidate Monetization-Basic");
                await Task.Delay(TimeSpan.FromMinutes(10));
            }


            count = await _SaleManager.SMSAlertApplyLimitCountForBilling("Candidate Monetization-Standard");
            if (count > 100)
            {
                await SalePostMonetizationBasicAsync("Candidate Monetization-Standard");
                await Task.Delay(TimeSpan.FromMinutes(10));
            }


            count = await _SaleManager.SMSAlertApplyLimitCountForBilling("Candidate Monetization-Premium");
            if (count > 100)
            {
                await SalePostMonetizationBasicAsync("Candidate Monetization-Premium");
                await Task.Delay(TimeSpan.FromMinutes(10));
            }


            count = await _SaleManager.SMSAlertApplyLimitCountForBilling("Apply Limit(Job Fair)");
            if (count > 100)
            {
                await SalePostMonetizationBasicAsync("Apply Limit(Job Fair)");
                await Task.Delay(TimeSpan.FromMinutes(10));
            }


            count = await _SaleManager.SMSAlertApplyLimitCountForBilling("Apply Limit(Job Seeker)");
            if (count > 100)
            {
                await SalePostMonetizationBasicAsync("Apply Limit(Job Seeker)");
                await Task.Delay(TimeSpan.FromMinutes(10));
            }


            count = await _SaleManager.SMSAlertApplyLimitCountForBilling("SMS Alert(Job Seeker)");
            if (count > 100)
            {
                await SalePostMonetizationBasicAsync("SMS Alert(Job Seeker)");
                await Task.Delay(TimeSpan.FromMinutes(10));
            }


        }

        #endregion 


        public IActionResult AutoBilling()
        {
            ClaimsPrincipal claimusers = HttpContext.User;
            if (claimusers.Identity.IsAuthenticated)
                return View();

            else
                return RedirectToAction("Index", "Home");
        }
    }
}
