using AccountingSystem.Abstractions.BLL;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.Web.Controllers
{
    public class LedgerController : Controller
    {
        private readonly ILedgerManager _LegerManager;

        public LedgerController(ILedgerManager LedgerManager)
        {
            _LegerManager = LedgerManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetService(int sTypy)
        {
            var result = await _LegerManager.GetService(sTypy);

            return Json(result);
        }
        public async Task<IActionResult> GetAllLedger()
        {
            var isAccount = HttpContext.Session.GetInt32("AccountDep").ToString();
            var isAdmin = HttpContext.Session.GetInt32("CanModifyAdmin").ToString();

            var result = await _LegerManager.GetAllLedger(isAdmin, isAccount);


            return Json(result);
        }
        public async Task<IActionResult> GetOnlineLedgerId(string onlineProduct)
        {
            var data = await _LegerManager.GetOnlineLedgerId(onlineProduct);

            return Json(data);
        }
        public async Task<IActionResult> GetProducts(int admin, int account, string groupname, string isAll, string isI, int isVatType)
        {
            var data = await _LegerManager.GetProducts(admin, account, groupname, isAll, isI, isVatType);

            return Json(data);
        }
        public async Task<IActionResult> GetLedgersWithBalance()
        {
            var data = await _LegerManager.GetLedgersWithBalance();

            return Json(data);
        }

    }
}
