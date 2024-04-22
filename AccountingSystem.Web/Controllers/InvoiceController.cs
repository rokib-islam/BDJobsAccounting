using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Models.AccountViewModels;
using AccountingSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace AccountingSystem.Web.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IInvoiceManager _InvoiceManager;
        private readonly HttpClient _httpClient;


        public InvoiceController(IInvoiceManager InvoiceManagerManager, HttpClient httpClient)
        {
            _InvoiceManager = InvoiceManagerManager;
            _httpClient = httpClient;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetInvoices(int cpId, string sDate, int ledgerId)
        {
            var data = await _InvoiceManager.GetInvoices(cpId, sDate, ledgerId);

            return Json(data);
        }
        //public async Task<IActionResult> UploadInvoicesOnline(int? cpId, string invoiceNo, int serviceNo, string billingContact, string price, string opId, string jpIdList, int serviceID, int companyID, string companyName, string saleDate)
        //{
        //    var invSendDt = await _InvoiceManager.GetInvSendDt(invoiceNo);
        //    var cid = cpId ?? 0;
        //    var results = await _InvoiceManager.UploadInvoiceOnline(cid, invoiceNo, serviceNo, invSendDt, billingContact, price, opId, jpIdList, serviceID, companyID, companyName, saleDate);

        //    return Json(results);
        //}

        [HttpPost]
        public async Task<IActionResult> UploadInvoicesOnline([FromBody] UploadInvoicesOnlineModel model)
        {
            var invSendDt = await _InvoiceManager.GetInvSendDt(model.InvoiceNo);
            var cid = model.CpId ?? 0;
            var results = await _InvoiceManager.UploadInvoiceOnline(cid, model.InvoiceNo, model.ServiceNo, invSendDt, model.BillingContact, model.Price, model.OpId, model.JpIdList, model.ServiceID, model.CompanyID, model.CompanyName, model.SaleDate);

            return Json(results);
        }
        public async Task<IActionResult> UpdateInvoice(string invoiceNo)
        {
            var results = await _InvoiceManager.UpdateInvoice(invoiceNo);

            return Json(results);
        }
        public async Task<IActionResult> SaveInvoice([FromBody] SaveInvoiceViewModel data)
        {
            var results = await _InvoiceManager.SaveInvoice(data);

            return Json(results);
        }
        public async Task<IActionResult> UpdateDeleteComments([FromBody] UpdateCommentViewModel data)
        {
            var results = await _InvoiceManager.UpdateDeleteComments(data);

            return Json(results);
        }
        public async Task<IActionResult> GetInvoicesForCashCollection(string query)
        {
            var results = await _InvoiceManager.GetInvoicesForCashCollectionAsync(query);

            return Json(results);
        }
        public async Task<IActionResult> PostToOnline(string postType, string invoiceNo, string invoiceId)
        {
            var results = await _InvoiceManager.PostToOnlineAsync(postType, invoiceNo, invoiceId);

            return Json(results);
        }
        public async Task<IActionResult> GetProductsForInvoice(string cId, int type)
        {
            var results = await _InvoiceManager.Getproducts(cId, type);

            return Json(results);
        }
        public async Task<IActionResult> GenerateInvoiceNumber(string cId, string issueDate)
        {
            var results = await _InvoiceManager.GenerateInvoiceNumberAsync(cId, issueDate);

            return Json(results);
        }
        public async Task<IActionResult> GetProductsDetails(string id)
        {
            var results = await _InvoiceManager.GetProductsDetails(id);

            return Json(results);
        }

        public IActionResult MakeInvoice(string companyId)
        {
            ClaimsPrincipal claimsPrincipal = HttpContext.User;
            if (claimsPrincipal.Identity.IsAuthenticated)
            {
                ViewBag.OnlineCompanyId = companyId;
                return View();
            }

            else
                return RedirectToAction("Index", "Home");
        }


        public Task<bool> CheckInvoiceNo(string invoiceNo)
        {
            var results = _InvoiceManager.CheckInvoiceNo(invoiceNo);

            return results;
        }

        public async Task<IActionResult> DeleteUndeleteInvoice(int invoiceId, bool invalid)
        {
            var results = await _InvoiceManager.DeleteUndeleteInvoice(invoiceId, invalid);

            return Json(results);
        }
        public async Task<IActionResult> UpdateAmount(string invoiceNo, decimal amount)
        {
            var results = await _InvoiceManager.UpdateAmount(invoiceNo, amount);

            return Json(results);
        }
        public async Task<IActionResult> GetInvoiceList(GetInvoiceListParam parameters)
        {
            var results = await _InvoiceManager.GetInvoicesAsync(parameters);

            return Json(results);
        }

        [HttpPost]
        [Route("api/OnlineInvocie")]
        public async Task<IActionResult> OnlineInvocie([FromBody] OnlineInvoiceRequestModel OnlineInvoice)
        {
            var responseList = await _InvoiceManager.OnlineInvcoie(OnlineInvoice);

            return await Task.FromResult(Ok(responseList));
        }

        public IActionResult ViewJournal()
        {
            ClaimsPrincipal claimusers = HttpContext.User;
            if (claimusers.Identity.IsAuthenticated)
                return View();

            else
                return RedirectToAction("Index", "Home");
        }

        public IActionResult OnlineInvoice()
        {
            ClaimsPrincipal claimsPrincipal = HttpContext.User;
            if (claimsPrincipal.Identity.IsAuthenticated)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> LoadOnlineInvoice([FromBody] LoadOnlineInvoiceModel model)
        {
            var result = await _InvoiceManager.LoadOnlineInvoice(model);
            return Json(result);
        }

        public async Task<IActionResult> OIvoiceReconc(int JP_ID)
        {
            var url = "https://corporate3.bdjobs.com/InvoiceReconciliation.asp";

            // No conversion needed, JP_ID remains an integer
            var content = new FormUrlEncodedContent(new[]
            {
                 new KeyValuePair<string, string>("ItemId", JP_ID.ToString())
             });

            try
            {
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // Handle successful response with potential API-specific processing
                    string responseMessage;
                    using (var responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            responseMessage = reader.ReadToEnd();
                            ApiResponseReconc responseObjectTyped = JsonConvert.DeserializeObject<ApiResponseReconc>(responseMessage);

                            if (responseObjectTyped.Status == 200)
                                return Json("Successfully posted");
                            else
                                return Json(responseObjectTyped.Message);
                        }
                    }
                }
                else
                {
                    return Json($"Invoice reconciliation failed (HTTP status code: {response.StatusCode})");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        public IActionResult OnlinePaymentVarification()
        {
            ClaimsPrincipal claimusers = HttpContext.User;
            if (claimusers.Identity.IsAuthenticated)
                return View();

            else
                return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> LoadOnlinePaymentVarificationData([FromBody] loadOnlinePaymentVarificationDataModel model)
        {
            var url = "https://corporate3.bdjobs.com/api/GetBillingsForAccouting.asp";

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("fromDate", model.FromDate),
                new KeyValuePair<string, string>("toDate", model.ToDate),
                new KeyValuePair<string, string>("verified", model.Verified),
                new KeyValuePair<string, string>("pageNo", model.PageNo.ToString()),
                new KeyValuePair<string, string>("pageSize", model.PageSize.ToString())
            });

            try
            {
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseMessage = await response.Content.ReadAsStringAsync();
                    ApiResponseModel responseObjectTyped = JsonConvert.DeserializeObject<ApiResponseModel>(responseMessage);

                    return Json(responseObjectTyped);
                }
                else
                {
                    return Json($"Onlin Payment Varification failed (HTTP status code: {response.StatusCode})");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }


        public async Task<IActionResult> VarifyOrReject([FromBody] VarifyOrReject model)
        {
            var url = "https://corporate3.bdjobs.com/api/GetBillingsForAccouting.asp";

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("QID", model.QID.ToString()),
                new KeyValuePair<string, string>("verified", model.verified.ToString()),
                new KeyValuePair<string, string>("verifiedBy", model.verifiedBy.ToString()),
                
            });

            try
            {
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseMessage = await response.Content.ReadAsStringAsync();
                    ApiResponseModel responseObjectTyped = JsonConvert.DeserializeObject<ApiResponseModel>(responseMessage);

                    return Json(responseObjectTyped);
                }
                else
                {
                    return Json($"Onlin Payment Varification failed (HTTP status code: {response.StatusCode})");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        public IActionResult CashCollection()
        {
            ClaimsPrincipal claimsPrincipal = HttpContext.User;
            if (claimsPrincipal.Identity.IsAuthenticated)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }



    }
}
