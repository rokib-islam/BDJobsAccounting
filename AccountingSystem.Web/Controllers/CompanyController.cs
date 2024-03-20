using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AccountingSystem.Web.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyManager _CompanyManager;

        public CompanyController(ICompanyManager CompanyManager)
        {
            _CompanyManager = CompanyManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetDistricts()
        {
            var result = await _CompanyManager.GetDistricts();

            return Json(result);
        }
        public async Task<IActionResult> GetOnlineCompanyList(int radio)
        {
            var result = await _CompanyManager.GetOnlineCompanyList(radio);

            return Json(result);
        }
        public async Task<IActionResult> GetCompanyListByKey(string startingKey)
        {
            var result = await _CompanyManager.GetCompanyListByKey(startingKey);
            return Json(result);
        }

        public async Task<IActionResult> GetOnlineCompanyInfo(int cpId)
        {
            var result = await _CompanyManager.GetOnlineCompanyInfo(cpId);

            return Json(result);
        }
        public async Task<IActionResult> GetLocalCompanyInfo(int cpId)
        {
            var result = await _CompanyManager.GetCompanyById(cpId);

            return Json(result);
        }
        public async Task<IActionResult> CheckCompany(string name)
        {
            var result = await _CompanyManager.CheckCompany(name);

            return Json(result);
        }

        public async Task<IActionResult> InsertUpdateOnlineCompany([FromBody] CompanyInsertUpdateViewModel FromData)
        {
            var result = await _CompanyManager.InsertUpdateOnlineCompany(FromData);
            await _CompanyManager.UpdateProfile(FromData);

            return Json(result);
        }
        public async Task<IActionResult> CheckOnlineCompany(int id)
        {
            var result = await _CompanyManager.CheckOnlineCompany(id);

            return Json(result);
        }
        public async Task<IActionResult> SMSAlertGetOnlineCompanyInfo(int cpId)
        {
            var result = await _CompanyManager.SMSAlertGetOnlineCompanyInfoAsync(cpId);

            return Json(result);
        }
        public async Task<IActionResult> SMSAlertGetOnlineCompany(int radio)
        {
            var result = await _CompanyManager.SMSAlertGetOnlineCompanyListAsync(radio);

            return Json(result);
        }
        public async Task<IActionResult> GetContactPersonsOrJobTitle(string type, int? cId)
        {
            var result = await _CompanyManager.GetContactPersonsOrJobTitle(type, cId);

            return Json(result);
        }
        public async Task<IActionResult> GetContactPersonByCompanyId(int companyId)
        {
            var result = await _CompanyManager.GetContactPersonsByCompanyId(companyId);

            return Json(result);
        }
        public async Task<IActionResult> GetContactPersonById(int id)
        {
            var result = await _CompanyManager.GetContactPersonByIdAsync(id);

            return Json(result);
        }
        public async Task<IActionResult> AddOrUpdatePerson(ContactPerson aPerson)
        {
            if (aPerson.Id == 0)
            {
                await _CompanyManager.InsertOrUpdateCPAsync(aPerson, "I");
            }
            else
            {
                await _CompanyManager.InsertOrUpdateCPAsync(aPerson, "U");
            }

            var returnValue = await _CompanyManager.GetContactPersonsByCompanyId(aPerson.CId);
            returnValue = returnValue.OrderBy(x => x.Name).ToList(); // Ensure the result is ordered.

            return Json(returnValue);
        }
        public async Task<IActionResult> DeletePerson(int Id, int CId)
        {
            await _CompanyManager.DeletePersonAsync(Id);

            var returnValue = await _CompanyManager.GetContactPersonsByCompanyId(CId);
            returnValue = returnValue.OrderBy(x => x.Name).ToList();

            return Json(returnValue);
        }
        public async Task<IActionResult> CheckCompanyNameWithId(string name, int id)
        {
            var resp = await _CompanyManager.GetCompanyByNameAsync(name, id) == null;
            return Json(resp);
        }
        public async Task<IActionResult> Index(Company aCompany)
        {
            var resp = await _CompanyManager.InsertOrUpdateCompanyAsync(aCompany);
            return Json(resp);
        }
        public async Task<IActionResult> Delete(int companyId)
        {
            await _CompanyManager.DeleteCompanyAsync(companyId);

            return Json(true);
        }
        public async Task<IActionResult> GetCompanyById(int companyId)
        {
            var resp = await _CompanyManager.GetCompanyById(companyId);
            return Json(resp);
        }

        public IActionResult Company()
        {
            ClaimsPrincipal claimusers = HttpContext.User;
            if (claimusers.Identity.IsAuthenticated)
                return View();

            else
                return RedirectToAction("Index", "Home");
        }
    }
}
