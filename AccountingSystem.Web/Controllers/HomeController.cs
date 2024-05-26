using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

            if (claimusers.Identity.IsAuthenticated && HttpContext.Session.GetString("Name") != null)
                return RedirectToAction("AccountingHome", "Home");
            else
                return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel credentials)
        {

            var user = await _AccountManager.GetUsers(credentials.username, credentials.password);
            if (user != null)
            {
                HttpContext.Session.SetString("Name", user.Name);
                HttpContext.Session.SetInt32("UserID", user.UserID);
                HttpContext.Session.SetString("AccessRight", user.AccessRight);
                HttpContext.Session.SetString("ApproveRight", user.ApproveRight);
                HttpContext.Session.SetInt32("AccountDep", user.AccountDep);
                HttpContext.Session.SetInt32("CanModifyAdmin", user.CanModifyAdmin);

                List<Claim> claims = new List<Claim>() {

                    new Claim(ClaimTypes.NameIdentifier ,user.UName),
                    new Claim("Id",user.UserID.ToString()),
                    new Claim("Name",user.Name),
                    new Claim("Designation",user.Designation),
                    new Claim("Email",user.Email),
                    new Claim("Mobile",user.MobileNo),
                    new Claim("SignatureImage",user.SignatureImage)
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {

                    AllowRefresh = false,
                    IsPersistent = credentials.rememberMe
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
            var userNameClaim = claimusers.FindFirst(ClaimTypes.Name) ?? claimusers.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
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

        public async Task<List<Users>> GetApprovers()
        {
            return await _AccountManager.GetApprovers();
        }
    }
}
