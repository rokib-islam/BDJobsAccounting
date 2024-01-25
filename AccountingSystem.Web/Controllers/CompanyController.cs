﻿using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;

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

            return View(result);
        }
        public async Task<IActionResult> GetOnlineCompanyList(int radio)
        {
            var result = await _CompanyManager.GetOnlineCompanyList(radio);

            return View(result);
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



    }
}
