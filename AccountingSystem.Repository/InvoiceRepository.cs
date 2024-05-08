using AccountingSystem.Abstractions.Repository;
using AccountingSystem.AppLicationDbContext.AccountingDatabase;
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
            var ChallanInfo = new ChallandetailsForOnlinePosting();

            try
            {
                using (var onlineSqlConnection = new SqlConnection(_DBCon.GetConnectionString("OnlineConnection")))
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


                            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                            {
                                ChallanInfo = await _db.QueryFirstOrDefaultAsync<ChallandetailsForOnlinePosting>("USP_ChallanInfo_For_OnlinePosting", new { InvoiceNo = invoiceNo }, commandType: CommandType.StoredProcedure);
                            }




                            var insertSql = "INSERT INTO dbo_Invoice(Cp_id,Inv_Date,Invoice_No,ServiceId,Bill_Contact,Amount,OPID) VALUES(@CpId, @InvSendDt, @InvoiceNo, @ServiceNo, @BillingContact, @Price, @OpId)";
                            await onlineSqlConnection.ExecuteAsync(insertSql, new
                            {
                                CpId = cpId,
                                InvSendDt = invSendDt,
                                InvoiceNo = invoiceNo,
                                ServiceNo = serviceNo,
                                BillingContact = billingContact,
                                Price = price,
                                OpId = opId
                            });

                            var dbo_Invoice_Id = await onlineSqlConnection.QueryFirstOrDefaultAsync<int>("SELECT InvoiceId FROM dbo_Invoice where INVOICE_NO=@InvoiceNo", new { InvoiceNo = invoiceNo });

                            var UniqueQuationNo = await onlineSqlConnection.QueryFirstOrDefaultAsync<string>("SELECT CONVERT(varchar, GETDATE(), 112) + '-' +CAST(SUM(CASE WHEN CP_ID = 112846 THEN 1 ELSE 0 END) + 1 AS varchar) AS totalCompanyQuotationSerial FROM PaymentQuotation", new { InvoiceNo = invoiceNo });

                            var QuatationinsertSql = "INSERT INTO PaymentQuotation(CP_ID, QuotationNo, ItemID, ItemType, TotalAmount)  VALUES (@CP_ID, @QuotationNo,@ItemID, @ItemType,  @TotalAmount)";
                            await onlineSqlConnection.ExecuteAsync(QuatationinsertSql, new
                            {
                                CP_ID = cpId,
                                QuotationNo = UniqueQuationNo,
                                ItemID = dbo_Invoice_Id,
                                ItemType = "Invoice",
                                TotalAmount = price,
                            });

                            var Quation_Id = await onlineSqlConnection.QueryFirstOrDefaultAsync<int>("select PQ_ID from PaymentQuotation where ItemID=@Invoice_Id", new { Invoice_Id = dbo_Invoice_Id });

                            var ChallaninsertSql = "INSERT INTO ChallanInfo(ChallanNo,CP_ID,CompanyName,CompanyVatAddress,CompanyBINNo,IssueDate,InvoiceNo,CreatedBy,CreaterDesignation ,ServiceName,UnitSupply,Quantity,UnitPrice, UnitTotalPrice,VatRate,VatAmount,UnitTotalPriceAll,TotalPrice,TotalVatPrice,TotalPriceAll,IsActive,PostedOn,PQ_ID,InvoiceComments)  VALUES (@ChallanNo,@CP_ID, @CompanyName,@CompanyVatAddress, @CompanyBINNo,  @IssueDate,  @InvoiceNo, @CreatedBy,  @CreaterDesignation,@ServiceName,  @UnitSupply,  @Quantity, @UnitPrice,        @UnitTotalPrice,  @VatRate,  @VatAmount, @UnitTotalPriceAll, @TotalPrice, @TotalVatPrice, @TotalPriceAll,  @IsActive, @PostedOn,@PQ_ID,@InvoiceComments )";
                            await onlineSqlConnection.ExecuteAsync(ChallaninsertSql, new
                            {
                                ChallanNo = ChallanInfo.ChallanNo,
                                CP_ID = cpId,
                                CompanyName = ChallanInfo.Contact_Person,
                                CompanyVatAddress = ChallanInfo.Address,
                                CompanyBINNo = ChallanInfo.VATRegNo,
                                IssueDate = ChallanInfo.Date,
                                InvoiceNo = ChallanInfo.InvoiceNo,
                                CreatedBy = ChallanInfo.UserName,
                                CreaterDesignation = ChallanInfo.UserDegignation,
                                ServiceName = ChallanInfo.ServiceName,
                                UnitSupply = ChallanInfo.ServiceType,
                                Quantity = ChallanInfo.Quantity,
                                UnitPrice = ChallanInfo.OneItemPrice,
                                UnitTotalPrice = ChallanInfo.TotalPrice,
                                VatRate = ChallanInfo.VatRate,
                                VatAmount = ChallanInfo.TotalVat,
                                UnitTotalPriceAll = ChallanInfo.TotalPriceAll,
                                TotalPrice = ChallanInfo.PriceWithVat,
                                TotalVatPrice = ChallanInfo.TotalVatAll,
                                TotalPriceAll = ChallanInfo.priceWithVatAll,
                                IsActive = 1,
                                PostedOn = ChallanInfo.Date,
                                PQ_ID = Quation_Id,
                                InvoiceComments = ChallanInfo.Comments,
                            });


                            var IntjpIdList = jpIdList.Split(',').Select(int.Parse).ToList();

                            var updateSql = "UPDATE JobBillInfo SET InvoiceID=(SELECT InvoiceId FROM dbo_Invoice WHERE Invoice_No=@InvoiceNo) WHERE jp_id IN @JpIdList";
                            await onlineSqlConnection.ExecuteAsync(updateSql, new { InvoiceNo = invoiceNo, JpIdList = IntjpIdList });

                            using (var onlineSqlConnection2 = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                            {
                                await onlineSqlConnection2.ExecuteAsync("UPDATE tmpJobs SET Submitted=1 WHERE Invoice_No = @InvoiceNo", new { InvoiceNo = invoiceNo });
                            }


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
                isOk = false;
                message = ex.ToString();
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
        public async Task<IEnumerable<InvoiceViewModel>> GetInvoicesForCashCollectionAsync(string query)
        {
            var invoices = new List<InvoiceViewModel>();

            var sqquery = "usp_GetInvoiceList " + query;

            var parameters = new { };

            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var result = await _db.QueryAsync<InvoiceViewModel>(sqquery, parameters, commandType: CommandType.Text);

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
        public async Task<List<ProductForInvoice>> Getproducts(string cId, int type)
        {
            var products = new List<ProductForInvoice>();


            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new { MakeView = type, CID = cId };
                    var result = await _db.QueryAsync("USP_GET_INVOICE_INVSCHEDULE", parameters, commandType: CommandType.StoredProcedure);

                    if (result.Any())
                    {
                        foreach (var item in result)
                        {
                            var product = new ProductForInvoice
                            {
                                Id = item.id,
                                ListItem = item.ListItem,
                                //SbName = item.SbName
                            };

                            if (type == 0)
                            {
                                product.Comments = item.Comments;
                                product.Amount = Convert.ToDouble(item.Amount);
                                product.SbName = item.SBName;
                                product.EDate = Convert.ToDateTime(item.EDate).ToShortDateString();
                                product.Product = item.Product;
                                product.LedgerId = item.LedgerID;
                                product.SDate = Convert.ToDateTime(item.SDate).ToShortDateString();
                                product.TNO = item.TNO;
                            }
                            else
                            {
                                product.SDate = Convert.ToDateTime(item.InvSendDt).ToShortDateString();
                                product.Amount = Convert.ToDouble(item.TAmount);
                                product.SbName = item.invoice_no;
                            }

                            products.Add(product);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return products;
        }
        public async Task<string> GenerateInvoiceNumberAsync(string cId, string issueDate)
        {
            var invoiceNumber = "";

            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new
                    {
                        CID = cId,
                        IssueDate = issueDate
                    };

                    invoiceNumber = await _db.QueryFirstOrDefaultAsync<string>("USP_SET_INVOICE_NUMBER_V1", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                throw ex;
            }

            return invoiceNumber;
        }
        public async Task<List<InvoiceViewModel>> GetProductsDetails(string id)
        {
            var invoices = new List<InvoiceViewModel>();

            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new { InvoiceID = id };
                    invoices = (await _db.QueryAsync<InvoiceViewModel>("USP_GET_INVOICE_DETAIL", parameters, commandType: CommandType.StoredProcedure)).ToList();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                throw ex;
            }

            return invoices;
        }
        public async Task<string> DeleteUndeleteInvoice(int invoiceId, bool invalid)
        {
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {

                    var rowsAffected = await _db.ExecuteAsync("UPDATE InvoiceList SET Invalid = @Invalid WHERE Id = @InvoiceId",
                        new { Invalid = invalid, InvoiceId = invoiceId });

                    if (rowsAffected > 0)
                    {
                        return "Invoice updated successfully.";
                    }
                    else
                    {
                        return "Invoice not found or no changes made.";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error updating invoice: {ex.Message}";
            }
        }
        public async Task<string> UpdateAmount(string invoiceNo, decimal amount)
        {
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {

                    var rowsAffected = await _db.ExecuteAsync("UPDATE InvoiceList SET TAmount = @Amount WHERE Invoice_No = @InvoiceNo",
                        new { Amount = amount, InvoiceNo = invoiceNo });

                    if (rowsAffected > 0)
                    {
                        return "Amount updated successfully.";
                    }
                    else
                    {
                        return "Invoice not found or no changes made.";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error updating amount: {ex.Message}";
            }
        }
        public async Task<List<InvoiceViewModel>> GetInvoicesAsync(GetInvoiceListParam parameters)
        {
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("TestConnection")))
                {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PageNo", parameters.PageNo);
                    dynamicParameters.Add("@PageSize", parameters.PageSize);
                    dynamicParameters.Add("@ProductID", parameters.ProductId);
                    dynamicParameters.Add("@Validity", parameters.Validity);
                    dynamicParameters.Add("@Operator", parameters.Operator);
                    dynamicParameters.Add("@FDuration", parameters.FromDuration);
                    dynamicParameters.Add("@TDuration", parameters.ToDuration);
                    dynamicParameters.Add("@FullPayment", parameters.FullPayment);
                    dynamicParameters.Add("@BlackListed", parameters.Blacklisted);
                    dynamicParameters.Add("@Order", parameters.Order);
                    dynamicParameters.Add("@Location", parameters.Location);
                    dynamicParameters.Add("@SalesPersonID", parameters.Salesperson);
                    dynamicParameters.Add("@FromRange", parameters.FromRange);
                    dynamicParameters.Add("@ToRange", parameters.ToRange);


                    var invoices = await _db.QueryAsync<InvoiceViewModel>(
                        "USP_LIST_OF_INVOICE_V1",
                        dynamicParameters,
                        commandType: CommandType.StoredProcedure);

                    return invoices.AsList();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions accordingly
                throw new Exception("Error retrieving invoices.", ex);
            }
        }

        public async Task<OnlineInvoiceResponseModel> OnlineInvcoie(OnlineInvoiceRequestModel parameters)
        {
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@acc_id", parameters.Acc_Id);
                    dynamicParameters.Add("@JP_ID", parameters.Jp_Id);
                    dynamicParameters.Add("@AddType", parameters.AddType);
                    dynamicParameters.Add("@Regional", parameters.Regional);
                    dynamicParameters.Add("@BlueCollar", parameters.BlueCollar);
                    dynamicParameters.Add("@SalesPrice", parameters.SalesPrice);
                    dynamicParameters.Add("@Tax", parameters.Tax);
                    dynamicParameters.Add("@SDate", parameters.SDate);
                    dynamicParameters.Add("@EDate", parameters.EDate);
                    dynamicParameters.Add("@SalesPersonName", parameters.SalesPersonName);
                    dynamicParameters.Add("@billingContact", parameters.BillingContact);
                    dynamicParameters.Add("@Designation", parameters.Designation);
                    dynamicParameters.Add("@Title", parameters.Title);
                    dynamicParameters.Add("@companyName", parameters.CompanyName);
                    dynamicParameters.Add("@address", parameters.Address);
                    dynamicParameters.Add("@city", parameters.City);
                    dynamicParameters.Add("@phone", parameters.Phone);
                    dynamicParameters.Add("@email", parameters.Email);
                    dynamicParameters.Add("@CP_ID", parameters.Cp_Id);
                    dynamicParameters.Add("@DistrictID", parameters.DistrictId);
                    dynamicParameters.Add("@BINNo", parameters.BINNo);
                    dynamicParameters.Add("@TransactionNo", parameters.TransactionNo);
                    dynamicParameters.Add("@PaymentMode", parameters.PaymentMethod);
                    dynamicParameters.Add("@JType", parameters.JType);


                    var invoices = await _db.QueryAsync<OnlineInvoiceResponseModel>(
                        "USP_OnlineInvoice",
                        dynamicParameters,
                        commandType: CommandType.StoredProcedure);

                    return invoices.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions accordingly
                throw new Exception("Error retrieving invoices.", ex);
            }
        }
        public async Task<OnlineInvoiceResponseModel> OnlineInvcoietest(OnlineInvoiceRequestModel parameters)
        {
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("TestConnection")))
                {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@acc_id", parameters.Acc_Id);
                    dynamicParameters.Add("@JP_ID", parameters.Jp_Id);
                    dynamicParameters.Add("@AddType", parameters.AddType);
                    dynamicParameters.Add("@Regional", parameters.Regional);
                    dynamicParameters.Add("@BlueCollar", parameters.BlueCollar);
                    dynamicParameters.Add("@SalesPrice", parameters.SalesPrice);
                    dynamicParameters.Add("@Tax", parameters.Tax);
                    dynamicParameters.Add("@SDate", parameters.SDate);
                    dynamicParameters.Add("@EDate", parameters.EDate);
                    dynamicParameters.Add("@SalesPersonName", parameters.SalesPersonName);
                    dynamicParameters.Add("@billingContact", parameters.BillingContact);
                    dynamicParameters.Add("@Designation", parameters.Designation);
                    dynamicParameters.Add("@Title", parameters.Title);
                    dynamicParameters.Add("@companyName", parameters.CompanyName);
                    dynamicParameters.Add("@address", parameters.Address);
                    dynamicParameters.Add("@city", parameters.City);
                    dynamicParameters.Add("@phone", parameters.Phone);
                    dynamicParameters.Add("@email", parameters.Email);
                    dynamicParameters.Add("@CP_ID", parameters.Cp_Id);
                    dynamicParameters.Add("@DistrictID", parameters.DistrictId);
                    dynamicParameters.Add("@BINNo", parameters.BINNo);
                    dynamicParameters.Add("@TransactionNo", parameters.TransactionNo);
                    dynamicParameters.Add("@PaymentMode", parameters.PaymentMethod);
                    dynamicParameters.Add("@JType", parameters.JType);


                    var invoices = await _db.QueryAsync<OnlineInvoiceResponseModel>(
                        "USP_OnlineInvoice",
                        dynamicParameters,
                        commandType: CommandType.StoredProcedure);

                    return invoices.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions accordingly
                throw new Exception("Error retrieving invoices.", ex);
            }
        }

        public async Task<CashCollectionAutoReponse> AutoCashCollection(CashCollectionAutoViewModel parameters)
        {
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@InvoiceNo", parameters.InvoiceNo);
                    dynamicParameters.Add("@SalesPrice", parameters.SalesPrice);
                    dynamicParameters.Add("@DiscountedPrice", parameters.DiscountedPrice);
                    dynamicParameters.Add("@PaymentMode", parameters.PaymentMode);
                    dynamicParameters.Add("@TransactionNo", parameters.TransactionNo);
                    dynamicParameters.Add("@SDate", parameters.SDate);
                    dynamicParameters.Add("@CP_Id", parameters.CP_Id);



                    var invoices = await _db.QueryAsync<CashCollectionAutoReponse>(
                        "USP_Auto_CashCollection",
                        dynamicParameters,
                        commandType: CommandType.StoredProcedure);

                    return invoices.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions accordingly
                throw new Exception("Error retrieving invoices.", ex);
            }
        }
        public async Task<CashCollectionAutoReponse> AutoCashCollectiontest(CashCollectionAutoViewModel parameters)
        {
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("TestConnection")))
                {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@InvoiceNo", parameters.InvoiceNo);
                    dynamicParameters.Add("@SalesPrice", parameters.SalesPrice);
                    dynamicParameters.Add("@DiscountedPrice", parameters.DiscountedPrice);
                    dynamicParameters.Add("@PaymentMode", parameters.PaymentMode);
                    dynamicParameters.Add("@TransactionNo", parameters.TransactionNo);
                    dynamicParameters.Add("@SDate", parameters.SDate);
                    dynamicParameters.Add("@CP_Id", parameters.CP_Id);



                    var invoices = await _db.QueryAsync<CashCollectionAutoReponse>(
                        "USP_Auto_CashCollection",
                        dynamicParameters,
                        commandType: CommandType.StoredProcedure);

                    return invoices.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions accordingly
                throw new Exception("Error retrieving invoices.", ex);
            }
        }

        public async Task<List<LoadOnlineInvoiceResponseModel>> LoadOnlineInvoice(LoadOnlineInvoiceModel model)
        {
            try
            {
                var parameters = new
                {
                    Status = model.Status,
                    Fromdate = model.FromDate,
                    Todate = model.ToDate,
                    PaymentMode = model.paymentMode,
                    ServiceName = model.ServiceName
                };
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var result = await _db.QueryAsync<LoadOnlineInvoiceResponseModel>("USP_LoadOnlineInvoice", parameters, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public async Task<int> CheckOrderIdCountAsync(string invoiceNo)
        {
            int orderCount = 0;
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    orderCount = await _db.QueryFirstOrDefaultAsync<int>("SELECT COUNT(*) FROM dbo.InvoiceList WHERE Invoice_No = @InvoiceNo AND DtOrderCode IS NOT NULL", new { InvoiceNo = invoiceNo });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return orderCount;
        }
        public async Task UpdateOrderInvoiceTableAsync(string invoiceNo, string courierOrderId, int userId)
        {
            try
            {
                string sqlQuery = "UPDATE dbo.InvoiceList SET DtOrderCode = @CourierOrderId,DtOrderUseId=@UserId WHERE Invoice_No = @InvoiceNo";
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    await _db.ExecuteAsync(sqlQuery, new { CourierOrderId = courierOrderId, InvoiceNo = invoiceNo, UserId = userId });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<LoadBouncedCheckDataModel>> LoadBouncedCheckData(LoadBouncedCheckDataModel model)
        {
            try
            {
                var parameters = new
                {
                    Fromdate = model.FromDate,
                    Todate = model.ToDate,
                    CId = model.CId,

                };
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var result = await _db.QueryAsync<LoadBouncedCheckDataModel>("USP_LoadAllBouncedCheckData", parameters, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

    }

}



