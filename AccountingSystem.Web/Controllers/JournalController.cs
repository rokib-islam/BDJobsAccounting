using AccountingSystem.Abstractions.BLL;
using Microsoft.AspNetCore.Mvc;

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
            //if (User.Identity.IsAuthenticated)
            //    return View();
            //else
            //return RedirectToAction("Index", "Home");
            return View();
        }

        public async Task<IActionResult> VireJournal(DateTime fromDate, DateTime toDate)
        {
            var result = await _journalManager.VireJournal(fromDate, toDate);

            // You can pass the result to the view or process it as needed

            return View(result);
        }

        public async Task<IActionResult> get(DateTime fromDate, DateTime toDate)
        {
            var result = await _journalManager.GetJournalListAsync(fromDate, toDate);

            // You can pass the result to the view or process it as needed

            return View(result);
        }
    }
}