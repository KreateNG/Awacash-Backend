using System;
namespace Awacash.Application.Customers.DTOs
{
    public class AccountDTO
    {
        public string? AccountNumber { get; set; }
        public string? AccountType { get; set; }
        public string? AccountStatus { get; set; }
        public string? AvailableBalance { get; set; }
        public string? LedgerBalance { get; set; }
        public string? WithdrawableAmount { get; set; }
    }
}

