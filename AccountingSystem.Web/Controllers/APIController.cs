﻿using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Models.AccountViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.Web.Controllers
{
    public class APIController : Controller
    {
        private readonly IInvoiceManager _InvoiceManager;
       


        public APIController(IInvoiceManager InvoiceManagerManager)
        {
            _InvoiceManager = InvoiceManagerManager;
           
        }

        [HttpPost]
        [Route("api/OnlineInvocie")]
        public async Task<IActionResult> OnlineInvocie([FromBody] OnlineInvoiceRequestModel OnlineInvoice)
        {
            var responseList = await _InvoiceManager.OnlineInvcoie(OnlineInvoice);

            return await Task.FromResult(Ok(responseList));
        }
        [HttpPost]
        [Route("api/OnlineInvocie-test")]
        public async Task<IActionResult> OnlineInvocietest([FromBody] OnlineInvoiceRequestModel OnlineInvoice)
        {
            var responseList = await _InvoiceManager.OnlineInvcoietest(OnlineInvoice);

            return await Task.FromResult(Ok(responseList));
        }

        [HttpPost]
        [Route("api/AutoCashCollection")]
        public async Task<IActionResult> AutoCashCollection([FromBody] CashCollectionAutoViewModel OnlineInvoice)
        {
            var responseList = await _InvoiceManager.AutoCashCollection(OnlineInvoice);

            return await Task.FromResult(Ok(responseList));
        }
        [HttpPost]
        [Route("api/AutoCashCollection-test")]
        public async Task<IActionResult> AutoCashCollectiontestTest([FromBody] CashCollectionAutoViewModel OnlineInvoice)
        {
            var responseList = await _InvoiceManager.AutoCashCollectiontest(OnlineInvoice);

            return await Task.FromResult(Ok(responseList));
        }

        [HttpPost]
        [Route("api/OnlineInvocie_For_Payment_Doc")]
        public async Task<IActionResult> OnlineInvocie_For_Payment_Doc([FromBody] OnlineInvoiceRequestModel OnlineInvoice)
        {
            var responseList = await _InvoiceManager.OnlineInvocie_For_Payment_Doc(OnlineInvoice);

            return await Task.FromResult(Ok(responseList));
        }

        [HttpPost]
        [Route("api/OnlineInvocie_For_Payment_Doc_test")]
        public async Task<IActionResult> OnlineInvocie_For_Payment_Doc_test([FromBody] OnlineInvoiceRequestModel OnlineInvoice)
        {
            var responseList = await _InvoiceManager.OnlineInvocie_For_Payment_Doc_test(OnlineInvoice);

            return await Task.FromResult(Ok(responseList));
        }

        [HttpPost]
        [Route("api/AutoCashCollection_For_Payment_Doc")]
        public async Task<IActionResult> AutoCashCollection_For_Payment_Doc([FromBody] CashCollectionAutoViewModel OnlineInvoice)
        {
            var responseList = await _InvoiceManager.AutoCashCollection_For_Payment_Doc(OnlineInvoice);

            return await Task.FromResult(Ok(responseList));
        }
        [HttpPost]
        [Route("api/AutoCashCollection_For_Payment_Doc_test")]
        public async Task<IActionResult> AutoCashCollection_For_Payment_Doc_test([FromBody] CashCollectionAutoViewModel OnlineInvoice)
        {
            var responseList = await _InvoiceManager.AutoCashCollection_For_Payment_Doc_test(OnlineInvoice);

            return await Task.FromResult(Ok(responseList));
        }
    }
}