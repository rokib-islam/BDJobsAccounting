﻿using AccountingSystem.Abstractions.Repository;
using AccountingSystem.AppLicationDbContext.AccountingDatabase;
using AccountingSystem.Models.AccountDbModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace AccountingSystem.Repository
{
    public class JournalRepository : IJournalRepository
    {
        private readonly AccountingDbContext _context;
        private readonly IConfiguration _DBCon;

        public JournalRepository(AccountingDbContext context, IConfiguration dbcon) //: base(context)
        {
            _context = context;
            _DBCon = dbcon;
        }

        public async Task<List<Journal>> GetJournalListAsync(int pageNo, int pageSize, int isPreview, string DateType, string startDate, string endDate, int LedgerId, string ledgerName, int companyId, int approvedBy, int postedBy, int isApproved)
        {
            var parameters = new
            {
                Page = new SqlParameter("@Page", pageNo),
                PageSize = new SqlParameter("@PageSize", pageSize),
                Preview = new SqlParameter("@Preview", isPreview),
                DateType = new SqlParameter("@DateType", DateType),
                StartDate = new SqlParameter("@StartDate", startDate),
                EndDate = new SqlParameter("@EndDate", endDate),
                LedgerId = new SqlParameter("@LedgerId", LedgerId),
                LedgerName = new SqlParameter("@LedgerName", ledgerName),
                CompanyId = new SqlParameter("@CompanyId", companyId),
                ApprovedBy = new SqlParameter("@ApprovedBy", approvedBy),
                PostedBy = new SqlParameter("@PostedBy", postedBy),
                IsApproved = new SqlParameter("@IsApproved", isApproved)
            };

            using (var _db = new SqlConnection(_DBCon.GetConnectionString("DefaultConnection")))
            {
                var result = await _db.QueryAsync<Journal>("USP_VIEW_JOURNAL_LIST", parameters, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

    }
}