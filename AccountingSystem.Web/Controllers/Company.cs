using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.Web.Controllers
{
    public class Company : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
