using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Models.AccountViewModels;
using AccountingSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AccountingSystem.Web.Controllers
{
    //[Authorize]
    public class JournalController : Controller
    {
        private readonly IJournalManager _journalManager;

        public JournalController(IJournalManager journalManager)
        {
            _journalManager = journalManager;
        }

        public IActionResult ViewJournal()
        {
            ClaimsPrincipal claimusers = HttpContext.User;
            if (claimusers.Identity.IsAuthenticated)
                return View();

            else
                return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> GetJournals([FromBody] GetJournalViewModel data)
        {
            var result = await _journalManager.GetJournalListAsync(data.PageNo, data.PageSize, data.IsPreview, data.DateType, data.StartDate, data.EndDate, data.LedgerId, data.LedgerName, data.CompanyId, data.ApprovedBy, data.PostedBy, data.IsApproved);

            return Json(result);
        }
        public async Task<IActionResult> GetClosingDate()
        {
            var result = await _journalManager.GetClosingDateAsync();

            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSalesJournal([FromBody] UpdateSalesJournal updateInfo)
        {
            var result = await _journalManager.UpdateSalesJournalAsync(updateInfo);
            return Json(result);
        }
        public async Task<IActionResult> GetVouchers(int year, int month)
        {
            var result = await _journalManager.GetVoucherListAsync(year, month);
            return Json(result);
        }

        public async Task<IActionResult> GetJournalsForTrialBalance([FromBody] GetJournalsForTrialBalanceModel model)
        {
            var result = await _journalManager.GetJournalsForTrialBalance(model.PageNo, model.PageSize, model.Tno, model.FromDate, model.EndDate);

            return Json(result);
        }

        public IActionResult MakeJournal()
        {
            ClaimsPrincipal claimusers = HttpContext.User;
            if (claimusers.Identity.IsAuthenticated)
                return View();

            else
                return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> GetMaxJournalId()
        {
            var result = await _journalManager.GetMaxJournalId();

            return Json(result);
        }



        public async Task<IActionResult> SaveJournalUpdateLedgerMakeVoucher([FromBody] List<JouralView> journals)
        {
            var result= await _journalManager.SaveJournalsAsync(journals);
            await _journalManager.MakeJournalVoucherAsync(result, journals[0].PostDate.ToString());

            return Json(result);
        }


        //public async Task<IActionResult> MakeJournalVoucherAsync(int jId, string postDate)
        //{
        //    var result = await _journalManager.MakeJournalVoucherAsync(jId, postDate);

        //    return Json(isSuccess);
        //}

    }
}