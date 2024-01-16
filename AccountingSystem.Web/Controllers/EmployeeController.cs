using AccountingSystem.Abstractions.BLL;
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
    }
}
