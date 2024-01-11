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
        public JsonResult Login(string userName, string password, bool rememberme)
        {
            var resultData = new { success = true, message = "Login successful" };

            var user = _AccountManager.GetUsers(userName, password);

            if (user != null)
            {
                //var date = ControllerContext.HttpContext.Request.Cookies["date"];
                //var shouldRun = false;
                //if (date == null)
                //    shouldRun = true;
                //else
                //{
                //    if (DateTime.Parse(date.Value) < DateTime.Today)
                //        shouldRun = true;
                //}
                //if (shouldRun)
                //{
                //    //DeleteTempFiles();
                //    var dateCookie = new HttpCookie("date") { Value = DateTime.Today.ToShortDateString() };
                //    this.ControllerContext.HttpContext.Response.Cookies.Add(dateCookie);
                //    dateCookie.Expires = DateTime.Now.AddDays(30);
                //}
                //Session["loggedinUser"] = user;

                //var s = rememberme ? "1" : "0";
                //var remember = new HttpCookie("isSaved") { Value = s };
                //var useridCookie = new HttpCookie("userid") { Value = user.UserId.ToString() };
                //this.ControllerContext.HttpContext.Response.Cookies.Add(useridCookie);
                //var usernameCookie = new HttpCookie("username") { Value = user.UserName };
                //var passwordCookie = new HttpCookie("password") { Value = user.Password };
                //this.ControllerContext.HttpContext.Response.Cookies.Add(usernameCookie);
                //this.ControllerContext.HttpContext.Response.Cookies.Add(passwordCookie);
                //usernameCookie.Expires = DateTime.Now.AddDays(2);
                //passwordCookie.Expires = DateTime.Now.AddDays(2);
                //this.ControllerContext.HttpContext.Response.Cookies.Add(remember);
                //remember.Expires = DateTime.Now.AddDays(2);

                //useridCookie.Expires = DateTime.Now.AddDays(30);



                return Json(resultData);
            }

            resultData = new { success = false, message = "Error User Name or Password" };

            // Return the JsonResult
            return Json(resultData);

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
