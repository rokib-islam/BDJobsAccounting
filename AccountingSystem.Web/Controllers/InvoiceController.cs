using AccountingSystem.Abstractions.BLL;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.Web.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly ISaleManager _SaleManager;

        public InvoiceController(ISaleManager SaleManager)
        {
            _SaleManager = SaleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
