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

        public async Task<IActionResult> GetDistricts()
        {
            var result = await _CompanyManager.GetDistricts();

            // You can pass the result to the view or process it as needed

            return View(result);
        }
        public async Task<IActionResult> GetOnlineCompanyList(int radio)
        {
            var result = await _CompanyManager.GetOnlineCompanyList(radio);

            // You can pass the result to the view or process it as needed

            return View(result);
        }

    }
}
