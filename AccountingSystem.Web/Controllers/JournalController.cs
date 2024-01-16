using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

            return View(result);
        }
    }
}