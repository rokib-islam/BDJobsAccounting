﻿using AccountingSystem.Models.AccountViewModels;

namespace AccountingSystem.Abstractions.BLL
{
    public interface ILedgerManager
    {
        //Users GetUsers(string userName, string password);

        Task<List<ServiceViewModel>> GetService(int sTypy);

    }
}
