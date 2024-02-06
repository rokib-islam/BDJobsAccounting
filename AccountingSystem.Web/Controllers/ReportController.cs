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
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Show(string InvoiceNo, string format)
        {
            try
            {
                var reportData = await _ReportManager.GetInvoiceReportAsync(InvoiceNo);
                string reportPath = Path.Combine(_WebHostEnvironment.WebRootPath, "Reports", "rptViewInvoice.rdlc");

                var datatable = Helpers.ListiToDataTable(reportData);
                var localReport = new LocalReport(reportPath);
                localReport.AddDataSource("dsAccounting", datatable);

                byte[] reportBytes;

                if (format == "pdf")
                {
                    var res = localReport.Execute(RenderType.Pdf, 1, null, "application/pdf");
                    reportBytes = res.MainStream;
                    return File(reportBytes, "application/pdf", "invoice_report.pdf");
                }
                else if (format == "excel")
                {
                    var res = localReport.Execute(RenderType.Excel, 1, null, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
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

