using System;
namespace Awacash.Domain.Models.EasyPay
{
    public class BalanceEnquiryDto
    {
        public string? ResponseCode { get; set; }
        public string? SessionID { get; set; }
        public string? TransactionId { get; set; }
        public string? BankVerificationNumber { get; set; }
        public decimal AvailableBalance { get; set; }
    }
}

