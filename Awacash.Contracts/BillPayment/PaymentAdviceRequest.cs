using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Contracts.BillPayment
{
    public record PaymentAdviceRequest(string? AccountNumber, string? PaymentCode, string? CustomerId, string? CustomerMobile, string? CustomerEmail, string? Amount, string? Pin);
    public record WalletPaymentAdviceRequest(string? PaymentCode, string? CustomerId, string? CustomerMobile, string? CustomerEmail, string? Amount, string? Pin);
}
