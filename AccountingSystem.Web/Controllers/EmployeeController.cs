using AccountingSystem.Abstractions.BLL;
using AccountingSystem.BLL;
using AccountingSystem.Models.AccountViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        public IActionResult EmployeeInfo()
        {
            ClaimsPrincipal claimusers = HttpContext.User;
            if (claimusers.Identity.IsAuthenticated)
                return View();

            else
                return RedirectToAction("Index", "Home");
        }

        public async Task<List<Department_Function_Rank_Model>> LoadAllDepartment()
        {
            var result = await _EmployeeManager.LoadAllDepartment();
            return result;
        }

        public async Task<List<Department_Function_Rank_Model>> LoadAllFunction()
        {
            var result = await _EmployeeManager.LoadAllFunction();
            return result;
        }

        public async Task<List<Department_Function_Rank_Model>> LoadAllRank()
        {
            var result = await _EmployeeManager.LoadAllRank();
            return result;
        }

        public async Task<List<Department_Function_Rank_Model>> LoadSupervisor()
        {
            var result = await _EmployeeManager.LoadSupervisor();
            return result;
        }
    }
}
