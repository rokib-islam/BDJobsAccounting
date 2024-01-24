﻿using AccountingSystem.Abstractions.Repository;
using AccountingSystem.AppLicationDbContext.AccountingDatabase;
using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Models.AccountViewModels;
using Dapper;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<string> SaveInvoice([FromBody] SaveInvoiceViewModel data)
        {
            var result = "";

            var parameters = new
            {
                userId = data.UserId,
                ActionType = data.Action,
                CompanyID = data.CId,
                Invoice_No = data.Invoice,
                InvSendDt = data.IssueDate,
                InvAmount = data.TotalPrice,
                InvSchdIDs = data.IdList,
                Invoice_ID = data.InvoiceId
            };

            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    await _db.ExecuteAsync("USP_INSERT_INVOICE", parameters);
                }
                result = "Insert Successfully!";
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        public async Task<bool> CheckInvoiceNo(string invoiceNo)
        {
            bool isFound = false;

            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var result = await _db.QueryFirstOrDefaultAsync<int>(
                        "SELECT COUNT(*) FROM dbo.InvoiceList WHERE Invoice_No = @InvoiceNo",
                        new { InvoiceNo = invoiceNo }
                    );

                    isFound = result > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isFound;
        }
        public async Task<string> UpdateDeleteComments(UpdateCommentViewModel data)
        {
            try
            {
                var parameters = new
                {
                    Action = data.Action,
                    InvSchId = data.InvSchId,
                    InvoiceId = data.InvoiceId,
                    Amount = data.Amount,
                    Comments = data.Comments,
                    InvDate = data.SendDate
                };

                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    await _db.ExecuteAsync("USP_UPDATE_INVOICE", parameters, commandType: CommandType.StoredProcedure);
                    return "Success";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public async Task<IEnumerable<Invoice>> GetInvoicesForCashCollectionAsync(int CompanyId, int FullPayment, int Invalid)
        {
            var invoices = new List<Invoice>();

            var parameters = new
            {
                CompanyId = CompanyId,
                FullPayment = FullPayment,
                Invalid = Invalid
            };

            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var result = await _db.QueryAsync<Invoice>("usp_GetInvoiceList", parameters, commandType: CommandType.StoredProcedure);

                    invoices.AddRange(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return invoices;
        }

        public async Task<string> PostToOnlineAsync(string postType, string invoiceNo, string invoiceId)
        {
            var message = "";
            var recordCount = 0;


            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            using (var _Onlinedb = new SqlConnection(_DBCon.GetConnectionString("OnlineConnection")))
            {

                var onlineTransaction = _Onlinedb.BeginTransaction();
                var localTransaction = _db.BeginTransaction();

                try
                {
                    if (postType == "All")
                    {
                        var invoices = await _db.QueryAsync("Select Invoice_No, UploadedPaymentStatus from InvoiceList where UploadedPaymentStatus='No'");

                        foreach (var invoice in invoices)
                        {
                            recordCount++;

                            await _Onlinedb.ExecuteAsync("usp_Acc_UpdatePayStatus", new { StatusType = 1, InvoiceNo = invoice.Invoice_No.ToString() }, commandType: CommandType.StoredProcedure, transaction: onlineTransaction);
                        }

                        await _db.ExecuteAsync("UPDATE InvoiceList SET UploadedPaymentStatus='Yes' where UploadedPaymentStatus='No'", transaction: localTransaction);

                        message = $"Total found {recordCount}. All are uploaded successfully.";
                    }
                    else
                    {
                        await _Onlinedb.ExecuteAsync("usp_Acc_UpdatePayStatus", new { StatusType = 1, InvoiceNo = invoiceNo }, commandType: CommandType.StoredProcedure, transaction: onlineTransaction);

                        await _db.ExecuteAsync(
                            $"UPDATE InvoiceList SET UploadedPaymentStatus='Yes' where id={invoiceId}",
                            transaction: localTransaction
                        );

                        message = "Upload online successful.";
                    }

                    onlineTransaction.Commit();
                    localTransaction.Commit();
                }
                catch (Exception ex)
                {
                    onlineTransaction.Rollback();
                    localTransaction.Rollback();

                    if (postType != "All")
                    {
                        await _db.ExecuteAsync($"UPDATE InvoiceList SET UploadedPaymentStatus='No' where id={invoiceId}", transaction: localTransaction);
                    }

                    message = "Sorry, Payment status cannot be uploaded now.Internet connection is not available.You can upload these Payment statuses later.";

                }

                return message;
            }
        }





    }
}


