using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

//using System.Web.Mvc;

namespace AccountingSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountManager _AccountManager;


        public HomeController(ILogger<HomeController> logger, IAccountManager AccManager)
        {
            _logger = logger;
            _AccountManager = AccManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginViewModel credentials)
        {
            var resultData = new { success = false, message = "Incorrect User Name or Password" };

            var user = _AccountManager.GetUsers(credentials.username, credentials.password);

            if (user != null)
            {
                HttpContext.Session.SetString("Name", user.Name);
                HttpContext.Session.SetInt32("UserID", user.UserID);

                if (credentials.rememberMe)
                {
                    // Set username in a cookie
                    Response.Cookies.Append("username", user.UName, new CookieOptions { Expires = DateTime.Now.AddDays(7) });
                    Response.Cookies.Append("password", user.PWord, new CookieOptions { Expires = DateTime.Now.AddDays(7) });
                }



                var resul = new { success = true, redirectUrl = Url.Action("AccountingHome", "Home") };
                return Json(resul);
            }

            // Return the JsonResult
            return Json(resultData);
        }


        public IActionResult AccountingHome()
        {
            var username = HttpContext.Session.GetString("Name");
            ViewBag.Username = username;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
