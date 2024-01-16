using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

//using System.Web.Mvc;

namespace AccountingSystem.Web.Controllers
{
    //[Authorize]
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
            ClaimsPrincipal claimusers = HttpContext.User;
            if (claimusers.Identity.IsAuthenticated)
                return RedirectToAction("AccountingHome", "Home");
            else
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel credentials)
        {
            //var resultData = new { success = false, message = "Incorrect User Name or Password" };

            //var user = _AccountManager.GetUsers(credentials.username, credentials.password);

            //if (user != null)
            //{
            //    HttpContext.Session.SetString("Name", user.Name);
            //    HttpContext.Session.SetInt32("UserID", user.UserID);

            //    if (credentials.rememberMe)
            //    {
            //        // Set username in a cookie
            //        Response.Cookies.Append("username", user.UName, new CookieOptions { Expires = DateTime.Now.AddDays(7) });
            //        Response.Cookies.Append("password", user.PWord, new CookieOptions { Expires = DateTime.Now.AddDays(7) });
            //    }



            //    var resul = new { success = true, redirectUrl = Url.Action("AccountingHome", "Home") };
            //    return Json(resul);
            //}

            //// Return the JsonResult
            //return Json(resultData);


            var user =await _AccountManager.GetUsers(credentials.username, credentials.password);
            if (user != null) 
            {
                HttpContext.Session.SetString("Name", user.Name);
                HttpContext.Session.SetInt32("UserID", user.UserID);
                HttpContext.Session.SetString("AccessRight", user.AccessRight);
                HttpContext.Session.SetString("ApproveRight", user.ApproveRight);

                List<Claim> claims = new List<Claim>() {

                    new Claim(ClaimTypes.NameIdentifier,user.UName),
                    //new Claim("Id",user.UserID.ToString())
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties=new AuthenticationProperties() { 
                
                    AllowRefresh = false,
                    IsPersistent= credentials.rememberMe
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

                //return RedirectToAction("AccountingHome", "Home");
                var resul = new { success = true, redirectUrl = Url.Action("AccountingHome", "Home") };
                return Json(resul);
            }
            return Json(new { success = false, message = "Incorrect User Name or Password" });
        }

        public async Task<IActionResult> LogOut()
        {
            HttpContext.Session.Clear();
           
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");


        }


        public IActionResult AccountingHome()
        {
            ClaimsPrincipal claimusers = HttpContext.User;
            if (claimusers.Identity.IsAuthenticated)
                return View();
            
            else
                return RedirectToAction("Index", "Home");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<List<Users>> GetSpecificUser()
        {
            return await _AccountManager.GetSpecificUser();
        }
    }
}
