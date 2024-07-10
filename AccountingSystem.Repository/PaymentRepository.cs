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
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AccountingDbContext _context;
        private readonly IConfiguration _DBCon;

        public PaymentRepository(AccountingDbContext context, IConfiguration config) //: base(context)
        {
            _context = context;
            _DBCon = config;
        }
        public async Task<IEnumerable<CashCollectionViewModel>> GetCashCollectionAsync(string id)
        {
            var collections = new List<CashCollectionViewModel>();

            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var query = @"SELECT id, cash, salesTax, receiveddate, posted, PaymentType, chequedetails, BadDebt, BankId
                          FROM cash_Collection
                          WHERE InvoiceSchedulerId = @Id";

                    var result = await _db.QueryAsync<CashCollectionViewModel>(query, new { Id = id });

                    collections.AddRange(result);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return collections;
        }
        public async Task<string> InsertCashCollectionAsync(InsertCashCollectionViewModel cashCollection)
        {
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new
                    {
                        Type = cashCollection.Type,
                        UserID = cashCollection.UserId,
                        Invoice_No = cashCollection.InvoiceNo,
                        Cash = cashCollection.Cash,
                        ReceivedDate = cashCollection.Date,
                        TNO = cashCollection.Tno,
                        InvoiceSchedulerID = cashCollection.InvoiceSchedulerId,
                        LedgerId = cashCollection.LedgerId,
                        ChequeDetails = cashCollection.ChequeDetails,
                        CompanyName = cashCollection.CompanyName
                    };

                    await _db.ExecuteAsync("USP_INSERT_CASH_COLLECTION", parameters, commandType: CommandType.StoredProcedure);

                    return "Success";
                }
            }
            catch (Exception ex)
            {
                return "An error occurred while inserting the cash collection.";
            }
        }
        public async Task<string> UpdateCashCollection(InsertCashCollectionViewModel cashCollection)
        {
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new
                    {
                        CCollectionID = cashCollection.CashCollectionId,
                        Type = cashCollection.Type,
                        UserID = cashCollection.UserId,
                        Invoice_No = cashCollection.InvoiceNo,
                        Cash = cashCollection.Cash,
                        ReceivedDate = cashCollection.Date,
                        TNO = cashCollection.Tno,
                        InvoiceSchedulerID = cashCollection.InvoiceSchedulerId,
                        LedgerId = cashCollection.LedgerId,
                        ChequeDetails = cashCollection.ChequeDetails,
                        CompanyName = cashCollection.CompanyName
                    };

                    await _db.ExecuteAsync("USP_UPDATE_CASH_COLLECTION", parameters, commandType: CommandType.StoredProcedure);

                    return "Success";
                }
            }
            catch (Exception ex)
            {
                return "An error occurred while updating the cash collection.";
            }
        }

        public async Task<string> UnpaidCashCollectionAsync(UnpaidCashCollection model)
        {
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new
                    {
                        UserID = model.UserId,
                        LedgerID = model.LedgerId,
                        TNO = model.Tno,
                        InvoiceId = model.InvoiceId,
                        InvoiceNo = model.InvoiceNo,
                        CollectionId = model.CollectionId,
                        Amount = model.Amount,
                        CompanyName = model.CompanyName
                    };

                    await _db.ExecuteAsync("USP_UNPAID_CASH_COLLECTION", parameters, commandType: CommandType.StoredProcedure);

                    return "Success";
                }
            }
            catch (Exception ex)
            {
                return "An error occurred while performing unpaid cash collection.";
            }
        }

        public async Task<List<BankInformationModel>> GetBankInformation()
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var result = await _db.QueryAsync<BankInformationModel>("Select * From BankInformation Order By BankID", new { });
                return result.ToList();
            }
        }

        public async Task<List<LoadPfPaymentDataResponseModel>> LoadPfPaymentData(LoadPfPaymentDataModel model)
        {
            try
            {
                var parameters = new
                {
                    Fromdate = model.FromDate,
                    Todate = model.ToDate,
                    EmployeeId = model.EmployeeId,
                };
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var result = await _db.QueryAsync<LoadPfPaymentDataResponseModel>("USP_LoadProvidentFundPaymentData", parameters, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<List<VatSectionModel>> GetVatSection()
        {
            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var result = await _db.QueryAsync<VatSectionModel>("SELECT Id, VatSectionName FROM VatSection", new { });
                return result.ToList();
            }
        }

        public async Task<List<VatSectionModel>> GetVatRateAsync(int id)
        {
            await using var db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection"));
            var result = await db.QueryAsync<VatSectionModel>(
                "SELECT Id, VatSectionName, VatRate FROM VatSection WHERE Id = @id",
                new { Id = id }
            );
            return result.ToList();
        }

        public async Task<string> InsertPaymentModule(PaymentModuleModel model)
        {
            var res = "";
            try
            {
                using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Date", model.Date);
                    parameters.Add("@PostingType", model.PostingType);
                    parameters.Add("@VendorId", model.VendorId);
                    parameters.Add("@ItemLedgerId", model.ItemLedgerId);
                    parameters.Add("@PayableLedgerId", model.PayableLedgerId);
                    parameters.Add("@BillReferenceNo", model.BillReferenceNo);
                    parameters.Add("@BillAmount", model.BillAmount);
                    parameters.Add("@VATSectionId", model.VATSectionId);
                    parameters.Add("@MushokNo", model.MushokNo);
                    parameters.Add("@MushokDate", model.MushokDate);
                    parameters.Add("@VATAmount", model.VATAmount);
                    parameters.Add("@TotalBill", model.TotalBill);
                    parameters.Add("@Narration", model.Narration);
                    //parameters.Add("@PaymentStatus", model.PaymentStatus);
                    //parameters.Add("@VarifiedById", model.VarifiedById);
                    //parameters.Add("@VarifiedDate", model.VarifiedDate);
                    parameters.Add("@EntryBy", model.EntryBy);
                    //parameters.Add("@EntryDate", model.EntryDate);
                    

                    await _db.ExecuteAsync("USP_InsertPaymentModuleInfo", parameters, commandType: CommandType.StoredProcedure);
                    res = "Success";
                }
            }
            catch (Exception ex)
            {
                res = ex.ToString();
            }
            return res;
        }

    }
}
