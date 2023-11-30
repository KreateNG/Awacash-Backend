using System;
namespace Awacash.Contracts.BillPayment
{
    public record AirTimePurchaseRequest(string? AccountNumber, string? PaymentCode, string? CustomerMobile, string? Amount, string? Pin);
    public record WalletAirTimePurchaseRequest(string? PaymentCode, string? CustomerMobile, string? Amount, string? Pin);
}

