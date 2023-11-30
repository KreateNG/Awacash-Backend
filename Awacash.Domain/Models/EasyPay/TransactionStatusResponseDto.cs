using System;
namespace Awacash.Domain.Models.EasyPay
{
    public class TransactionStatusResponseDto
    {
        public string? ResponseCode { get; set; }
        public string? SessionID { get; set; }
        public string? Amount { get; set; }
        public string? Charge { get; set; }
        public string? DebitAccountNumber { get; set; }
        public string? BeneficiaryAccountNumber { get; set; }
        public string? BeneficiaryBankCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? ResponseMessage { get; set; }
    }
}

