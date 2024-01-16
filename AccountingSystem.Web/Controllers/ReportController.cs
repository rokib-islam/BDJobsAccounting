using AccountingSystem.Abstractions.BLL;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportManager _ReportManager;

        public ReportController(IReportManager ReportManager)
        {
            _ReportManager = ReportManager;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
