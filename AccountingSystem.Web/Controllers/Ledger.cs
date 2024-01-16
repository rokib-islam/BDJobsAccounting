using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.Web.Controllers
{
    public class Ledger : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
