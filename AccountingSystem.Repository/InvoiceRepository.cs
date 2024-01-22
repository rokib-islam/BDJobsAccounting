﻿using AccountingSystem.Abstractions.Repository;
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
        public async Task<object> UploadInvoiceOnline(int cpId, string invoiceNo, int serviceNo, string invSendDt, string billingContact, string price, string opId, string jpIdList, int serviceID, int companyID, string companyName, string saleDate)
        {
            var isOk = true;
            var isExist = false;
            var message = "";

            try
            {
                using (var onlineSqlConnection = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var cmdtext = "SELECT * FROM dbo_Invoice WHERE Invoice_No=@InvoiceNo";
                    var data = await onlineSqlConnection.QueryFirstOrDefaultAsync(cmdtext, new { InvoiceNo = invoiceNo });

                    if (data != null)
                    {
                        isExist = true;
                    }
                    else
                    {
                        if (serviceNo <= 0)
                        {
                            if (serviceID > 0)
                                serviceNo = serviceID;
                            else
                                serviceNo = 11;

                            var insertSql = "INSERT INTO dbo_Invoice(Cp_id,Inv_Date,Invoice_No,ServiceId,Bill_Contact,Amount,OPID) VALUES(@CpId, @InvSendDt, @InvoiceNo, @ServiceNo, @BillingContact, @Price, @OpId)";
                            await onlineSqlConnection.ExecuteAsync(insertSql, new { CpId = cpId, InvSendDt = invSendDt, InvoiceNo = invoiceNo, ServiceNo = serviceNo, BillingContact = billingContact, Price = price, OpId = opId });

                            var updateSql = "UPDATE JobBillInfo SET InvoiceID=(SELECT InvoiceId FROM dbo_Invoice WHERE Invoice_No=@InvoiceNo) WHERE jp_id IN @JpIdList";
                            await onlineSqlConnection.ExecuteAsync(updateSql, new { InvoiceNo = invoiceNo, JpIdList = jpIdList });

                            await onlineSqlConnection.ExecuteAsync("UPDATE tmpJobs SET Submitted=1 WHERE Invoice_No = @InvoiceNo", new { InvoiceNo = invoiceNo });

                            message = $"Invoice number '<strong>{invoiceNo}</strong>' has been sent to the online database successfully.";
                        }
                        else
                        {
                            // Your existing code for the 'else' part...

                            if (companyID == 0 && companyName != "")
                            {
                                try
                                {
                                    var selectCompanySql = "SELECT cp_id FROM dbo_Company_Profiles WHERE name=@CompanyName";
                                    companyID = await onlineSqlConnection.QueryFirstOrDefaultAsync<int>(selectCompanySql, new { CompanyName = companyName });
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }

                            var insertSql = "INSERT INTO dbo_Invoice(Cp_id,Inv_Date,Invoice_No,ServiceId,Amount) VALUES(@CompanyID, @SaleDate, @InvoiceNo, @ServiceNo, @Price)";
                            await onlineSqlConnection.ExecuteAsync(insertSql, new { CompanyID = companyID, SaleDate = saleDate, InvoiceNo = invoiceNo, ServiceNo = serviceNo, Price = price });

                            using (var onlineSqlConnection2 = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                            {
                                await onlineSqlConnection2.ExecuteAsync("UPDATE InvoiceList SET UploadedPaymentStatus='Yes' WHERE Invoice_No=@InvoiceNo", new { InvoiceNo = invoiceNo });
                            }

                            message = $"The invoice '<strong>{invoiceNo}</strong>' is uploaded successfully.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception or log it as needed
                throw ex;
            }

            return new { Duplicate = isExist, Message = message, Connection = isOk };
        }


        public async Task<string> GetInvSendDt(string invoiceNo)
        {
            string invsendDate = "";

            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    await _db.OpenAsync();

                    // Use Dapper to execute the query asynchronously and map the result to invsendDate
                    invsendDate = await _db.QueryFirstOrDefaultAsync<string>(
                        "SELECT InvSendDt FROM InvoiceList WHERE invoice_no = @InvoiceNo",
                        new { InvoiceNo = invoiceNo }
                    );
                }
            }
            catch (Exception ex)
            {
                // Handle the exception or log it as needed
                throw ex;
            }

            return invsendDate;
        }

        public async Task<string> UpdateInvoice(string invoiceNo)
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {

                // Use Dapper to execute the update query asynchronously
                await _db.ExecuteAsync("UPDATE tmpJobs SET Submitted = 1 WHERE Invoice_No = @InvoiceNo", new { InvoiceNo = invoiceNo });
            }

            var message = $"The invoice '<strong>{invoiceNo}</strong>' is updated successfully.";
            return message;
        }


    }
}
