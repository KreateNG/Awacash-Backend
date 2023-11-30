using Awacash.Domain.Models.BillsPayment;
using Awacash.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.BillPayment.Services
{
    public interface IBillPaymentService
    {
        Task<ResponseModel<List<Biller>>> GetBillers();
        Task<ResponseModel<List<Biller>>> GetBillerByCategory(int categoryId);
        Task<ResponseModel<List<Paymentitem>>> GetBillerPaymentItems(string billerId);
        Task<ResponseModel<PaymentAdviceResponse>> SendPaymentAdvice(string accountNumber, string paymentCode, string customerId, string customerMobile, string customerEmail, string amount, string pin);
        Task<ResponseModel<PaymentAdviceResponse>> AirtimePurchase(string accountNumber, string paymentCode, string customerMobile, string amount, string pin);
        Task<ResponseModel<BillPaymentCustomer>> ValidateCustomer(string customerId, string paymentCode);
        Task<ResponseModel<BillerCategory>> GetBillerCatrgory();
        Task<ResponseModel<PaymentAdviceResponse>> WalletAirtimePurchase(string paymentCode, string customerMobile, string amount, string pin);
        Task<ResponseModel<PaymentAdviceResponse>> SendWalletPaymentAdvice(string paymentCode, string customerId, string customerMobile, string customerEmail, string amount, string pin);
    }
}
