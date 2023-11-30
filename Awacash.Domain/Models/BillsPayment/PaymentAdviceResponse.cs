using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.Models.BillsPayment
{
    public class PaymentAdviceResponse
    {
        public string? RechargePIN { get; set; }
        public string? PhcnTokenDetails { get; set; }
        public string? MiscData { get; set; }
        public string? TransactionRef { get; set; }
        public string? ResponseCode { get; set; }
        public string? ResponseMessage { get; set; }
        public string? ResponseCodeGrouping { get; set; }
        public AdditionalInfo? AdditionalInfo { get; set; }
    }

    public class AdditionalInfo
    {
        public string? CustomerAddress { get; set; }
        public string? ResetToken { get; set; }
        public string? ConfigureToken { get; set; }
        public string? Units { get; set; }
        public string? FreeUnits { get; set; }
        public string? Tariff { get; set; }
        public string? TariffBaseRate { get; set; }
        public string? DebtAmount { get; set; }
        public string? DebtRemaining { get; set; }
        public string? DebtCoverage { get; set; }
        public string? AmountGenerated { get; set; }
        public string? ReceiptNo { get; set; }
        public string? Tax { get; set; }
    }
}
