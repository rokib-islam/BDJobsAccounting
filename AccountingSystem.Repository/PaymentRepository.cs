using AccountingSystem.Abstractions.Repository;
using AccountingSystem.AppLicationDbContext.AccountingDatabase;
using AccountingSystem.Models.AccountDbModels;
using AccountingSystem.Models.AccountViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    }
}
