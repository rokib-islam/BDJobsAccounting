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

        public async Task<IActionResult> LoadAllDepartment()
        {
            var result = await _EmployeeManager.LoadAllDepartment();
            return Json(result);
        }

        public async Task<IActionResult> LoadAllFunction()
        {
            var result = await _EmployeeManager.LoadAllFunction();
            return Json(result);
        }

        public async Task<IActionResult> LoadAllRank()
        {
            var result = await _EmployeeManager.LoadAllRank();
            return Json(result);
        }

        public async Task<IActionResult> LoadSupervisor()
        {
            var result = await _EmployeeManager.LoadSupervisor();
            return Json(result);
        }

        public async Task<IActionResult> InsertOrUpdateEmployeeInfo([FromBody] EmployeeModel model)
        {
            var result = await _EmployeeManager.InsertOrUpdateEmployeeInfo(model);
            return Json(result);
        }

        public async Task<IActionResult> LoadEmployeeInfoById(int id)
        {
            var result = await _EmployeeManager.LoadEmployeeInfoById(id);
            return Json(result);
        }

        public async Task<IActionResult> InsertOrUpdateAcknowledgement([FromBody] Acknowledgement_GrossSalary_TA_Model model)
        {
            var result = await _EmployeeManager.InsertOrUpdateAcknowledgement(model);
            return Json(result);
        }

        public async Task<IActionResult> GetAcknowledgementNoByEmployeeId(int employeeId)
        {
            var result = await _EmployeeManager.GetAcknowledgementNoByEmployeeId(employeeId);
            return Json(result);
        }

        public async Task<IActionResult> InsertOrUpdateGrossSalary([FromBody] Acknowledgement_GrossSalary_TA_Model model)
        {
            var result = await _EmployeeManager.InsertOrUpdateGrossSalary(model);
            return Json(result);
        }

        public async Task<IActionResult> GetGrossSalaryByEmployeeId(int employeeId)
        {
            var result = await _EmployeeManager.GetGrossSalaryByEmployeeId(employeeId);
            return Json(result);
        }

        public async Task<IActionResult> InsertOrUpdateTA([FromBody] Acknowledgement_GrossSalary_TA_Model model)
        {
            var result = await _EmployeeManager.InsertOrUpdateTA(model);
            return Json(result);
        }

        public async Task<IActionResult> GetTaByEmployeeId(int employeeId)
        {
            var result = await _EmployeeManager.GetTaByEmployeeId(employeeId);
            return Json(result);
        }

        public async Task<IActionResult> LoadAllEmployeeInfo()
        {
            var result = await _EmployeeManager.LoadAllEmployeeInfo();
            return Json(result);
        }

        public async Task<IActionResult> ImportACS()
        {
            var result = await _EmployeeManager.ImportACS();
            return Json(result);
        }

        public IActionResult EmployeeSalaryPosting()
        {
            ClaimsPrincipal claimusers = HttpContext.User;
            if (claimusers.Identity.IsAuthenticated)
                return View();

            else
                return RedirectToAction("Index", "Home");
        }

    }
}
