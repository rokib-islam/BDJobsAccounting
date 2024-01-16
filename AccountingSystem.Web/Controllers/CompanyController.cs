using AccountingSystem.Abstractions.BLL;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.Web.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyManager _CompanyManager;

        public CompanyController(ICompanyManager CompanyManager)
        {
            _CompanyManager = CompanyManager;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
