using AccountingSystem.Abstractions.Repository;
using AccountingSystem.AppLicationDbContext.AccountingDatabase;
using AccountingSystem.Models.AccountViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace AccountingSystem.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly AccountingDbContext _context;
        private readonly IConfiguration _DBCon;

        public InvoiceRepository(AccountingDbContext context, IConfiguration config) //: base(context)
        {
            _context = context;
            _DBCon = config;
        }
        public async Task<List<InvoiceForOnlineJobViewModel>> GetInvoices(int cpId, string sDate, int ledgerId)
        {
            try
            {
                var invoices = new List<InvoiceForOnlineJobViewModel>();

                var parameters = new
                {
                    PCode = ledgerId,
                    InvoiceSentDate = sDate,
                    CP_ID = cpId
                };

                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    invoices = (await _db.QueryAsync<InvoiceForOnlineJobViewModel>("USP_UPLOAD_INVOICE_ONLINE", parameters, commandType: CommandType.StoredProcedure))
                                          .AsList();
                }

                return invoices;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
