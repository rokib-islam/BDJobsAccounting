using AccountingSystem.Abstractions.BLL;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentManager _PaymentManager;

        public PaymentController(IPaymentManager PaymentManager)
        {
            _PaymentManager = PaymentManager;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
