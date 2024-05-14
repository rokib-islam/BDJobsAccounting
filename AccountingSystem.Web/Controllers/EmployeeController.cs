using AccountingSystem.Abstractions.BLL;
using AccountingSystem.BLL;
using AccountingSystem.Models.AccountViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeManager _EmployeeManager;

        public EmployeeController(IEmployeeManager employeeManager)
        {
            _EmployeeManager = employeeManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetEmployeeListByKey(string startingKey)
        {
            var result = await _EmployeeManager.GetEmployeeListByKey(startingKey);
            return Json(result);
        }

        public async Task<IActionResult> InsertProvidentFundPayment([FromBody] InsertProvidentFundPaymentModel model)
        {
            var resp = await _EmployeeManager.InsertProvidentFundPayment(model);
            return Json(resp);
        }
    }
}
