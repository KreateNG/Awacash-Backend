
namespace Awacash.Domain.Models.EasyPay
{
    public class TransferResponseDto
    {
        public string? ResponseCode { get; set; }
        public string? SessionID { get; set; }
        public string? DestBankCode { get; set; }
        public string? SourceAccountNo { get; set; }
        public string? DestAccountNo { get; set; }
        public decimal TranAmount { get; set; }
        public decimal ChargeAmount { get; set; }
        public string? Narration { get; set; }
        public string? SenderName { get; set; }
        public string? ResponseMessage { get; set; }
    }
}

