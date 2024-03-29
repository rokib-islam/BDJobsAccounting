﻿using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Models.AccountViewModels;
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
        public async Task<IActionResult> GetCashCollection(string Id)
        {
            var result = await _PaymentManager.GetCashCollectionAsync(Id);

            return Json(result);
        }
        public async Task<IActionResult> InsertCashCollection([FromBody] InsertCashCollectionViewModel cashCollection)
        {
            var result = await _PaymentManager.InsertCashCollectionAsync(cashCollection);

            return Json(result);
        }
        public async Task<IActionResult> UpdateCashCollection([FromBody] InsertCashCollectionViewModel cashCollection)
        {
            var result = await _PaymentManager.UpdateCashCollection(cashCollection);

            return Json(result);
        }
        public async Task<IActionResult> UnpaidCashCollection([FromBody] UnpaidCashCollection cashCollection)
        {
            var result = await _PaymentManager.UnpaidCashCollectionAsync(cashCollection);

            return Json(result);
        }


    }
}
