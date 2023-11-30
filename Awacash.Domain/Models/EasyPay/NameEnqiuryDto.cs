using System;
namespace Awacash.Domain.Models.EasyPay
{
    public class NameEnqiuryDto
    {
        public string? ResponseMessage { get; set; }
        public string? ResponseCode { get; set; }
        public string? TransactionId { get; set; }
        public string? SessionID { get; set; }
        public string? AccountName { get; set; }
        public string? BankVerificationNumber { get; set; }
        public int KycLevel { get; set; }
    }


}

