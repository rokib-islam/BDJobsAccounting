using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public bool Login([FromBody] LoginViewModel credentials)
        {
            var resultData = new { success = false, message = "Incorrect User Name or Password" };

            var user = _AccountManager.GetUsers(credentials.username, credentials.password);

            if (user != null)
            {
                HttpContext.Session.SetString("loggedinUser", JsonConvert.SerializeObject(user));

                if (credentials.rememberMe)
                {
                    // Set username in a cookie
                    Response.Cookies.Append("username", user.UName, new CookieOptions { Expires = DateTime.Now.AddDays(7) });
                    Response.Cookies.Append("password", user.PWord, new CookieOptions { Expires = DateTime.Now.AddDays(7) });
                }


                RedirectToAction("ViewJournal", "Journal");
                return true;
            }

            // Return the JsonResult
            return false;

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
