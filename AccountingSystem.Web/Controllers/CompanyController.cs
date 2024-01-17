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

            return View(result);
        }
        public async Task<IActionResult> GetOnlineCompanyList(int radio)
        {
            var result = await _CompanyManager.GetOnlineCompanyList(radio);

            return View(result);
        }
        public async Task<IActionResult> GetCompanyListByKey(string startingKey)
        {
            var result = await _CompanyManager.GetCompanyListByKey(startingKey);

            return View(result);
        }


    }
}
