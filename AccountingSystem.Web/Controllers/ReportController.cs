using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Models.AccountViewModels;
using AccountingSystem.Web.HelperMethod;
using Microsoft.AspNetCore.Mvc;
//using AspNetCore.Reporting;
using Microsoft.Reporting.NETCore;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
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



        public async Task<IActionResult> ShowInvoiceList(string format, int isColorPad, GetInvoiceListParam param)
        {
            try
            {
                double totalAmount = 0;
                var reportData = await _InvoiceManager.GetInvoicesAsync(param);
                var fullPayment = reportData.FirstOrDefault().FullPayment;
                totalAmount = reportData.Sum(report => report.TAmount);

                string wordamount = await ConvertToWords((int)Math.Round(totalAmount));
                var datatable = Helpers.ListiToDataTable(reportData);

                using var report = new LocalReport();
                var parameters = new[]
                {
                new ReportParameter("SumAmount", totalAmount.ToString("N2")),
                new ReportParameter("AmountInWord", wordamount),
                new ReportParameter("isColorPad", isColorPad.ToString("0")),
                new ReportParameter("FullPayment", fullPayment.ToString())
            };

                using var rs = Assembly.GetExecutingAssembly().GetManifestResourceStream("AccountingSystem.Web.Reports.rptShowInvoiceList.rdlc");
                report.LoadReportDefinition(rs);
                report.DataSources.Add(new ReportDataSource("ShowInvoiceList", datatable));
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
                var courierOrderId = "";

                var reportData = await _ReportManager.GetLabelReport(type, list);

                var datatable = Helpers.ListiToDataTable(reportData);

                var userId = User.FindFirstValue("Id");

                var checkOrderCode = await _InvoiceManager.CheckOrderIdCountAsync(invoiceId);
                if (checkOrderCode == "" && dtChk == true)
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
                        courierOrderId = responseObject.model[0].courierOrderId.ToString();


                        await _InvoiceManager.UpdateOrderInvoiceTableAsync(invoiceId, courierOrderId, int.Parse(userId));
                    }
                }
                else
                {
                    courierOrderId = checkOrderCode;
                }

                if (courierOrderId != "")
                {
                    courierOrderId = "Order ID# " + courierOrderId;
                }

                using var report = new LocalReport();
                var parameters = new[]
                {
                    new ReportParameter("DTCode", courierOrderId),
                };

                using var rs = Assembly.GetExecutingAssembly().GetManifestResourceStream("AccountingSystem.Web.Reports.rptLabelPrevire.rdlc");
                report.LoadReportDefinition(rs);
                report.DataSources.Add(new ReportDataSource("PreviewLabel", datatable));
                report.SetParameters(parameters);

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

        public async Task<IActionResult> ReportMail(string InvoiceNo, string format, bool sMail, bool sChallan, string CopyType)
        {
            var userId = int.Parse(User.FindFirstValue("Id"));

            if (sMail == false && sChallan == false)
            {
                return StatusCode(200, "You did not select any Invoice or Challan.");
            }

            else if (userId == 28 || userId == 55 || userId == 47 || userId == 62 || userId == 105 || userId == 122 || userId == 79 || userId == 125 || userId == 120 || userId == 132 || userId == 129 || userId == 135 || userId == 126)
            {
                try
                {
                    // Retrieve user information
                    string Image = "";
                    var Designation = User.FindFirstValue("Designation");
                    var Email = User.FindFirstValue("Email");
                    var Mobile = User.FindFirstValue("Mobile");
                    var Name = User.FindFirstValue("Name");
                    var toMail = "";

                    byte[] InvoicefileContents = null;
                    string InvoicecontentType = null;
                    string InvoicefileName = null;

                    byte[] ChallanContents = null;
                    string ChallanContentType = null;
                    string ChallanFileName = null;



                    var assembly = Assembly.GetExecutingAssembly();

                    // List and log all resources for debugging
                    var resourceNames = assembly.GetManifestResourceNames();
                    foreach (var resourceName in resourceNames)
                    {
                        Console.WriteLine(resourceName);
                    }

                    if (sMail)
                    {
                        var reportData = await _ReportManager.GetInvoiceReportAsync(InvoiceNo);
                        Image = reportData.FirstOrDefault().SignatureImage;
                        string userImagePath = Path.Combine(_WebHostEnvironment.WebRootPath, "Images", Image);
                        toMail = reportData.FirstOrDefault().AccPersonMail;
                        double totalAmount = reportData.Sum(report => report.amount);
                        string wordamount = await ConvertToWords((int)Math.Round(totalAmount));
                        var datatable = Helpers.ListiToDataTable(reportData);

                        using var report = new LocalReport();
                        report.EnableExternalImages = true;

                        var parameters = new[]
                        {
                        new ReportParameter("SumAmount", totalAmount.ToString("N2")),
                        new ReportParameter("AmountInWord", wordamount),
                        new ReportParameter("isColorPad", "0"),
                        new ReportParameter("ImagePath", new Uri(userImagePath).AbsoluteUri)
                    };

                        using var rs = assembly.GetManifestResourceStream("AccountingSystem.Web.Reports.rptViewInvoiceForMail.rdlc");
                        if (rs == null)
                        {
                            return StatusCode(500, "Report template not found.");
                        }

                        report.LoadReportDefinition(rs);
                        report.DataSources.Add(new ReportDataSource("ShowInvoice", datatable));
                        report.SetParameters(parameters);

                        InvoicefileContents = report.Render("pdf");
                        InvoicecontentType = "application/pdf";
                        InvoicefileName = "InvoiceReport.pdf";

                        if (InvoicefileContents == null || InvoicefileContents.Length == 0)
                        {
                            return StatusCode(500, "Failed to generate report content.");
                        }
                    }

                    if (sChallan)
                    {
                        var reportData = await _ReportManager.GetChalanReportNew(InvoiceNo);
                        Image = reportData.FirstOrDefault().SignatureImage;
                        string userImagePath = Path.Combine(_WebHostEnvironment.WebRootPath, "Images", Image);
                        toMail = reportData.FirstOrDefault().AccPersonMail;
                        double sumTotalVat = reportData.Sum(report => report.TotalVat);
                        double sumPriceWithVat = reportData.Sum(report => report.priceWithVat);
                        double sumTotalPrice = reportData.Sum(report => report.TotalPrice);
                        var datatable = Helpers.ListiToDataTable(reportData);

                        using var report = new LocalReport();
                        report.EnableExternalImages = true;

                        var parameters = new[]
                        {
                        new ReportParameter("sumTotalVat", sumTotalVat.ToString("N2")),
                        new ReportParameter("sumPriceWithVat", sumPriceWithVat.ToString("N2")),
                        new ReportParameter("sumTotalPrice", sumTotalPrice.ToString("N2")),
                        new ReportParameter("CopyType", CopyType),
                        new ReportParameter("ImagePath", new Uri(userImagePath).AbsoluteUri)
                    };

                        using var rs = assembly.GetManifestResourceStream("AccountingSystem.Web.Reports.rptShowChallanForMail.rdlc");
                        if (rs == null)
                        {
                            return StatusCode(500, "Report template not found.");
                        }

                        report.LoadReportDefinition(rs);
                        report.DataSources.Add(new ReportDataSource("ShowChallan", datatable));
                        report.SetParameters(parameters);

                        ChallanContents = report.Render("pdf");
                        ChallanContentType = "application/pdf";
                        ChallanFileName = "ChallanReport.pdf";

                        if (ChallanContents == null || ChallanContents.Length == 0)
                        {
                            return StatusCode(500, "Failed to generate report content.");
                        }
                    }

                    var subject = $"Bill/Invoice #{InvoiceNo} from Bdjobs.com Ltd.";
                    var Body = $@"
                        <html>
                        <head>
                            <style>
                                table {{
                                    font-family: Arial, sans-serif;
                                    border-collapse: collapse;
                                    width: 100%;
                                    color: black;
                                }}
                                th, td {{
                                    border: 1px solid #dddddd;
                                    text-align: left;
                                    padding: 8px;
                                    color: black;
                                }}
                                th {{
                                    background-color: #f2f2f2;
                                }}
                                p {{
                                    color: black;
                                }}
                                .boldColor {{
                                    color: blue;
                                    font-weight: bold;
                                }}
                                .mobile {{
                                    font-weight: bold;
                                }}
                                .bkash {{
                                    font-weight: bold;
                                }}
                                .refColor {{
                                    color: blue;
                                }}
                                .userColor {{
                                    color: black;
                                }}
                            </style>
                        </head>
                        <body>
                            <p>Dear Sir,</p>
                            <p>Please find the attachment for the required invoice from Bdjobs.com Ltd. We are requesting you to pay the bill at your earliest (if not yet paid). You can pay our bill by the following ways:</p>
                            <p>Please pay by Bank Transfer/ BEFTN/ NPSB/ RTGS/ Direct Bank deposit to any of the following bank account.</p>
                            <table>
                                <tr>
                                    <th>Bank Name</th>
                                    <th>Account Title</th>
                                    <th>Bank Account Number</th>
                                    <th>Bank Routing Number</th>
                                    <th>Bank Branch Name</th>
                                    <th>Swift Code</th>
                                </tr>
                                <tr>
                                    <td>Southeast Bank Limited</td>
                                    <td>BDJOBS.COM LIMITED</td>
                                    <td>001513100000131</td>
                                    <td>205262535</td>
                                    <td>Kawran Bazar</td>
                                    <td>SEBDBDDHKRN</td>
                                </tr>
                                <tr>
                                    <td>United Commercial Bank PLC</td>
                                    <td>BDJOBS.COM LIMITED</td>
                                    <td>0441301000000430</td>
                                    <td>245262537</td>
                                    <td>Kawran Bazar</td>
                                    <td>UCBLBDDHKBZ</td>
                                </tr>
                                <tr>
                                    <td>Standard Chartered</td>
                                    <td>BDJOBS.COM LIMITED</td>
                                    <td>0001536942801</td>
                                    <td>215262538</td>
                                    <td>Kawran Bazar</td>
                                    <td>SCBLBDDX</td>
                                </tr>
                            </table>
                            <p>Also, you can pay to our <span class=""bkash"">bKash/ Nagad/ Upay</span> merchant account number: <span class=""mobile"">01841235627</span></p>
                            <p>Please write our invoice’s <span class=""refColor"">last 6-digit number as a payment reference</span> whenever paying by bKash/ Nagad/ Upay to recognize your payment and account it accordingly.</p>
                            <p class=""boldColor"">Please don’t forget to send us the payment reference (bank advice or transaction id) in reply mail (or to accounts@bdjobs.com) immediately to clear your outstanding.</p>
                            <p>We thank you for taking our services. For any query, please let us know to serve.</p>
                            <p>Best Regards,</p>
                            <p class=""userColor"">{Name}<br />{Designation}<br />{Email}<br />{Mobile}</p>
                        </body>
                        </html>
                        ";

                    var attachments = new List<(byte[] content, string name, string contentType)>
                {
                    (InvoicefileContents, InvoicefileName, InvoicecontentType),
                    (ChallanContents, ChallanFileName, ChallanContentType)
                }.Where(a => a.content != null).ToList(); // Only include non-null attachments

                    await SendEmailAsync(toMail, subject, Body, attachments);

                    return StatusCode(200, "E-Mail Sent Successfully!!!");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.ToString());
                }
            }
            else
            {
                return StatusCode(200, "You have no permission to send this mail.");
            }
        }


        private async Task SendEmailAsync(string toEmail, string subject, string body, List<(byte[] content, string name, string contentType)> attachments)
        {
            try
            {
                using (var message = new MailMessage())
                {
                    message.From = new MailAddress("accountsinfo@bdjobs.com");
                    message.To.Add(toEmail);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    // List to keep references to MemoryStreams to prevent them from being disposed
                    var attachmentStreams = new List<MemoryStream>();

                    foreach (var attachment in attachments)
                    {
                        if (attachment.content == null || attachment.content.Length == 0)
                        {
                            throw new InvalidOperationException("Attachment is null or empty.");
                        }

                        var stream = new MemoryStream(attachment.content);
                        attachmentStreams.Add(stream); // Keep the reference
                        var attachmentItem = new Attachment(stream, attachment.name, attachment.contentType);
                        message.Attachments.Add(attachmentItem);
                    }

                    using (var smtpClient = new SmtpClient("mail.bdjobs.com"))
                    {
                        smtpClient.Port = 587; // or use your SMTP port
                        smtpClient.Credentials = new NetworkCredential("accountsinfo@bdjobs.com", "Peq1KUza");
                        smtpClient.EnableSsl = false; // Use SSL if required

                        await smtpClient.SendMailAsync(message);
                    }

                    // Dispose streams after email is sent
                    foreach (var stream in attachmentStreams)
                    {
                        stream.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw new InvalidOperationException("Failed to send email.", ex);
            }
        }





    }
}

