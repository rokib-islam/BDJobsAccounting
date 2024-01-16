using AccountingSystem.Abstractions.BLL;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.Web.Controllers
{
    public class LedgerController : Controller
    {
        private readonly ILedgerManager _LegerManager;

        public LedgerController(ILedgerManager LedgerManager)
        {
            _LegerManager = LedgerManager;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
