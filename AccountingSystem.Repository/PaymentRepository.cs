using AccountingSystem.Abstractions.Repository;
using AccountingSystem.AppLicationDbContext.AccountingDatabase;
using Microsoft.Extensions.Configuration;

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
        //public Users GetUsers(string userName, string password)
        //{
        //    using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
        //    {
        //        // Use QueryFirstOrDefault instead of Query for getting a single result
        //        var result = _db.QueryFirstOrDefault<Users>("SELECT * FROM Users WHERE UName = @UName AND PWord = @PWord",
        //            new { UName = userName, PWord = password });

        //        return result;
        //    }
        //}


    }
}
