using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Models.AccountViewModels;
using AccountingSystem.Web.HelperMethod;
using Microsoft.AspNetCore.Mvc;
//using AspNetCore.Reporting;
using Microsoft.Reporting.NETCore;
using Newtonsoft.Json;
using System.Reflection;
using System.Security.Claims;
using System.Text;



namespace AccountingSystem.Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportManager _ReportManager;
        private readonly IInvoiceManager _InvoiceManager;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        public ReportController(IReportManager ReportManager, IInvoiceManager invoiceManager, IWebHostEnvironment WebHostEnv)
        {
            _ReportManager = ReportManager;
            _InvoiceManager = invoiceManager;
            _WebHostEnvironment = WebHostEnv;
        }
        private static string[] ones = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
        private static string[] teens = { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
        private static string[] tens = { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult TrailBalance()
        {

            ClaimsPrincipal claimusers = HttpContext.User;
            if (claimusers.Identity.IsAuthenticated)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }
        public async Task<string> ConvertToWords(int number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + await ConvertToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += await ConvertToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += await ConvertToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += await ConvertToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                if (number < 10)
                    words += ones[number];
                else if (number < 20)
                    words += teens[number - 10];
                else
                {
                    words += tens[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + ones[number % 10];
                }
            }

            return words;
        }


        public async Task<IActionResult> ShowInvoice(string InvoiceNo, string format, int isColorPad)
        {
            try
            {
                double totalAmount = 0;
                var reportData = await _ReportManager.GetInvoiceReportAsync(InvoiceNo);

                totalAmount = reportData.Sum(report => report.amount);

                string wordamount = await ConvertToWords((int)Math.Round(totalAmount));
                var datatable = Helpers.ListiToDataTable(reportData);

                using var report = new LocalReport();
                var parameters = new[]
                {
                new ReportParameter("SumAmount", totalAmount.ToString("N2")),
                new ReportParameter("AmountInWord", wordamount),
                new ReportParameter("isColorPad", isColorPad.ToString())
            };

                using var rs = Assembly.GetExecutingAssembly().GetManifestResourceStream("AccountingSystem.Web.Reports.rptViewInvoice.rdlc");
                report.LoadReportDefinition(rs);
                report.DataSources.Add(new ReportDataSource("ShowInvoice", datatable));
                report.SetParameters(parameters);

                byte[] fileContents;
                string contentType;
                string fileName;

                if (format.ToLower() == "pdf")
                {
                    fileContents = report.Render("pdf");
                    contentType = "application/pdf";
                    fileName = "InvoiceReport.pdf";
                }
                else if (format.ToLower() == "excel")
                {
                    fileContents = report.Render("excel");
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    fileName = "InvoiceReport.xlsx";
                }
                else
                {
                    // Handle unsupported format
                    return BadRequest("Unsupported format. Please specify either 'pdf' or 'excel'.");
                }

                return File(fileContents, contentType, fileName);
            }
            catch
            {
                return StatusCode(500, "An error occurred while generating the report.");
            }
        }
        public async Task<IActionResult> ShowChallanReportNew(string InvoiceNo, string format, string CopyType)
        {
            try
            {
                var reportData = await _ReportManager.GetChalanReportNew(InvoiceNo);
                double sumTotalVat = reportData.Sum(report => report.TotalVat);
                double sumPriceWithVat = reportData.Sum(report => report.priceWithVat);
                double sumTotalPrice = reportData.Sum(report => report.TotalPrice);
                var datatable = Helpers.ListiToDataTable(reportData);

                using var report = new LocalReport();
                var parameters = new[]
                {
                new ReportParameter("sumTotalVat", sumTotalVat.ToString("N2")),
                new ReportParameter("sumPriceWithVat", sumPriceWithVat.ToString("N2")),
                new ReportParameter("sumTotalPrice", sumTotalPrice.ToString("N2")),
                new ReportParameter("CopyType", CopyType),

                };

                using var rs = Assembly.GetExecutingAssembly().GetManifestResourceStream("AccountingSystem.Web.Reports.rptShowChallan.rdlc");
                report.LoadReportDefinition(rs);
                report.DataSources.Add(new ReportDataSource("ShowChallan", datatable));
                report.SetParameters(parameters);

                byte[] fileContents;
                string contentType;
                string fileName;

                if (format.ToLower() == "pdf")
                {
                    fileContents = report.Render("pdf");
                    contentType = "application/pdf";
                    fileName = "ChallanReport.pdf";
                }
                else if (format.ToLower() == "excel")
                {
                    fileContents = report.Render("excel");
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    fileName = "ChallanReport.xlsx";
                }
                else
                {
                    // Handle unsupported format
                    return BadRequest("Unsupported format. Please specify either 'pdf' or 'excel'.");
                }

                return File(fileContents, contentType, fileName);
            }
            catch
            {
                return StatusCode(500, "An error occurred while generating the report.");
            }
        }

        public async Task<IActionResult> ShowTrailBalance(string type, string startDate, string endDate, string format)
        {
            try
            {
                var reportData = await _ReportManager.GetTrialBalanceReportAsync(type, startDate, endDate);
                double sumDebit = reportData.Where(report => report.Total > 0).Sum(report => report.Total);
                double sumPCredit = Math.Abs(reportData.Where(report => report.Total < 0).Sum(report => report.Total));



                var datatable = Helpers.ListiToDataTable(reportData);

                var title = type == "Month" ? "For the Month of "
                    + new DateTime(Convert.ToInt32(endDate), Convert.ToInt32(startDate), 1).ToString("MMMM, yyyy") : "For the date " + startDate + " to " + endDate;

                using var report = new LocalReport();
                var parameters = new[]
                {
                    new ReportParameter("title", title),
                    new ReportParameter("sumDebit", sumDebit.ToString("N2")),
                    new ReportParameter("sumCredit", sumPCredit.ToString("N2"))
                };



                using var rs = Assembly.GetExecutingAssembly().GetManifestResourceStream("AccountingSystem.Web.Reports.rptTrailBalance.rdlc");
                report.LoadReportDefinition(rs);
                report.DataSources.Add(new ReportDataSource("ShowTrialBalance", datatable));
                report.SetParameters(parameters);

                byte[] fileContents;
                string contentType;
                string fileName;

                if (format.ToLower() == "pdf")
                {
                    fileContents = report.Render("pdf");
                    contentType = "application/pdf";
                    fileName = "TrialBalance.pdf";
                }
                else if (format.ToLower() == "excel")
                {
                    fileContents = report.Render("excel");
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    fileName = "TrialBalance.xlsx";
                }
                else
                {
                    // Handle unsupported format
                    return BadRequest("Unsupported format. Please specify either 'pdf' or 'excel'.");
                }

                return File(fileContents, contentType, fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while generating the report.");
            }
        }

        public async Task<IActionResult> PreviewLabelReport(string type, string list, string format, bool dtChk, string invoiceId)
        {
            try
            {
                var reportData = await _ReportManager.GetLabelReport(type, list);

                var datatable = Helpers.ListiToDataTable(reportData);

                var userId = User.FindFirstValue("Id");

                var checkOrderCode = await _InvoiceManager.CheckOrderIdCountAsync(invoiceId);
                if (checkOrderCode == 0 && dtChk == true)
                {
                    var result = await CallApiAsync();
                    dynamic responseModel = JsonConvert.DeserializeObject<dynamic>(result);
                    var token = responseModel.model.token.ToString();
                    var payload = new[]
                    {
                        new
                        {
                            customerName = reportData[0].pname + ", " +  reportData[0].designation,
                            customerMobile = "01752495580",
                            customerAltMobile = "",
                            customerAddress = reportData[0].cname + ", " + reportData[0].address + ", " + reportData[0].city,
                            districtId = reportData[0].DistrictID,
                            thanaId = 0,
                            areaId = 0,
                            collectAddressDistrictId = 14,
                            collectAddressThanaId = 16060,
                            collectAddress = "8th Floor - West BDBL Building Old BSRS 12 Kawran Bazar CA Dhaka-1215 Bangladesh",
                            referenceName = "",
                            collectionAmount = 0,
                            WeightRangeId = 32,
                            isBigParcel = false,
                            actualPackagePrice = 30,
                            collectionTimeSlotId = 2,
                            serviceType = "alltoall",
                            orderFrom = "BdJobs",
                            version = "",
                            note = ""
                        }
                    };

                    var jsonPayload = JsonConvert.SerializeObject(payload);
                    using (var httpClient = new HttpClient())
                    {
                        var apiEndpoint = "https://coreapi.deliverytiger.com.bd/api/ThirdParty/AddBulkOrder";
                        var apiKey = "7C4479ED-4539-488C-B6B8-20D4911AC04A";
                        httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                        var response = await httpClient.PostAsync(apiEndpoint, content);
                        response.EnsureSuccessStatusCode();
                        var jsonResponse = await response.Content.ReadAsStringAsync();

                        dynamic responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                        var courierOrderId = responseObject.model[0].courierOrderId.ToString();

                        await _InvoiceManager.UpdateOrderInvoiceTableAsync(invoiceId, courierOrderId, int.Parse(userId));
                    }
                }



                using var report = new LocalReport();

                using var rs = Assembly.GetExecutingAssembly().GetManifestResourceStream("AccountingSystem.Web.Reports.rptLabelPrevire.rdlc");
                report.LoadReportDefinition(rs);
                report.DataSources.Add(new ReportDataSource("PreviewLabel", datatable));

                byte[] fileContents;
                string contentType;
                string fileName;

                if (format.ToLower() == "pdf")
                {
                    fileContents = report.Render("pdf");
                    contentType = "application/pdf";
                    fileName = "PreviewLabel.pdf";
                }
                else if (format.ToLower() == "excel")
                {
                    fileContents = report.Render("excel");
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    fileName = "PreviewLabel.xlsx";
                }
                else if (format.ToLower() == "word")
                {

                    fileContents = report.Render("WORDOPENXML");
                    contentType = "application/msword";
                    fileName = "PreviewLabel.docx";
                }
                else
                {
                    // Handle unsupported format
                    return BadRequest("Unsupported format. Please specify either 'pdf' or 'excel'.");
                }

                return File(fileContents, contentType, fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while generating the report.");
            }
        }
        private async Task<string> CallApiAsync()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var apiEndpoint = "https://coreapi.deliverytiger.com.bd/api/Accounts/ThirdPartyUserLogin";
                    var apiKey = "7C4479ED-4539-488C-B6B8-20D4911AC04A";
                    httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
                    var requestBody = new
                    {
                        mobile = "01811410859",
                        password = "12345"
                    };
                    var jsonBody = JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(apiEndpoint, content);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request error: {e.Message}");
                    return null;
                }
            }
        }

        public IActionResult VatTaxCollectionReport()
        {

            ClaimsPrincipal claimusers = HttpContext.User;
            if (claimusers.Identity.IsAuthenticated)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> LoadVatTaxCollectionData([FromBody] LoadVatTaxCollectionDataModel_Request model)
        {
            var result = await _ReportManager.LoadVatTaxCollectionData(model);
            return Json(result);
        }
    }
}

