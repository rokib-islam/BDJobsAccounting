using AccountingSystem.Abstractions.BLL;
using AccountingSystem.Web.HelperMethod;
using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Show(string InvoiceNo, string format)
        {
            double totalAmount = 0;
            try
            {
                var reportData = await _ReportManager.GetInvoiceReportAsync(InvoiceNo);
                foreach (var report in reportData)
                {
                    totalAmount += report.amount;
                }
                string wordamount = await ConvertToWords((int)Math.Round(totalAmount));

                string reportPath = Path.Combine(_WebHostEnvironment.WebRootPath, "Reports", "rptViewInvoice.rdlc");
                var datatable = Helpers.ListiToDataTable(reportData);
                var localReport = new LocalReport(reportPath);
                localReport.AddDataSource("ShowInvoice", datatable);


                var parameters = new Dictionary<string, string>
                {
                    { "SumAmount", totalAmount.ToString() },
                    { "AmountInWord", wordamount }
                };


                byte[] reportBytes;

                if (format == "pdf")
                {
                    var res = localReport.Execute(RenderType.Pdf, 1, parameters, "application/pdf");
                    reportBytes = res.MainStream;
                    return File(reportBytes, "application/pdf", "invoice_report.pdf");
                }
                else if (format == "excel")
                {
                    var res = localReport.Execute(RenderType.Excel, 1, parameters, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    reportBytes = res.MainStream;
                    return File(reportBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "invoice_report.xlsx");
                }
                else
                {
                    return BadRequest("Invalid format specified.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while generating the report.");
            }
        }

    }
}

